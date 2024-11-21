using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudyAssist.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using Utilities.Interfaces;
using static IdentityModel.OidcConstants;
using static System.Net.WebRequestMethods;

namespace StudyAssist.KnowledgeDataConverter.Api
{
    internal class KnowledgeDataProvider : IKnowledgeDataProvider, IDisposable
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        private string _identityServerUri = @"AuthConfig:IdentityServerAuthorityUrl";

        private string _dataAccessUri = @"WebApiUrls:KnowledgeDataAccessApi";

        private HttpClient _httpClient;

        public KnowledgeDataProvider(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public Task<Catalog> SaveCatalogAsync(Catalog catalog)
        {
            ArgumentNullException.ThrowIfNull(catalog);

            Catalog addingCatalog = new()
            {
                Name = catalog.Name,
                CatalogId = 0
            };

            try
            {
                return _SaveItem<Catalog>(addingCatalog, "catalogs");
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException("Saving catalog error.", ex);
            }
        }

        private async Task<T> _SaveItem<T>(T addingItem, string controllerUriSegment)
        {
            ArgumentException.ThrowIfNullOrEmpty(controllerUriSegment);

            _SetHttpClient();

            HttpContent catalogContent = JsonContent.Create(addingItem);

            HttpResponseMessage response = await _httpClient.PostAsync(
                controllerUriSegment, catalogContent);

            if(response.IsSuccessStatusCode == false)
                throw new InvalidOperationException(
                    await _CreateErrorMessageFromResponse(response));

            T? addedCatalog = await response.Content.ReadFromJsonAsync<T>();

            if(addedCatalog == null)
                throw new InvalidOperationException(
                    "Null response result was received from dataAccessApi");

            return addedCatalog;
        }

        private async Task<string> _CreateErrorMessageFromResponse(HttpResponseMessage response)
        {
            ProblemDetails? problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            string probMsg = string.Empty;

            if(problem == null)
                return string.Empty;

            string errors = problem.Extensions
                .FirstOrDefault(ext => ext.Key == "errors")
                .Value?.ToString() ?? string.Empty;

            probMsg = $"Incorrect response was received from dataAccessApi:" +
                $" Status:{problem.Status}. Title: {problem.Title ?? string.Empty} {errors}";

            return probMsg;
        }

        private void _SetHttpClient()
        {
            if(_httpClient != null)
                return;
            
            _httpClient = _httpClientFactory.CreateClient();

            string? knowledgeDataApiBaseUri = _configuration.GetValue<string>(_dataAccessUri);

            if(knowledgeDataApiBaseUri is null || knowledgeDataApiBaseUri == string.Empty)
                throw new InvalidOperationException("DataAccess uri not found");

            _httpClient.BaseAddress = new Uri(knowledgeDataApiBaseUri);
        }

        public async Task<int> SaveCatalog(Catalog catalog)
        {
            //HttpClient identityServerClient = _httpClientFactory.CreateClient();

            //DiscoveryDocumentResponse discoveryDocument = await identityServerClient.GetDiscoveryDocumentAsync(
            //    _identityServerUri);

            //var tokenResponse = await identityServerClient.RequestClientCredentialsTokenAsync(
            //    new ClientCredentialsTokenRequest
            //    {
            //        Address = discoveryDocument.TokenEndpoint,
            //        ClientId = "studyAssistConverter_id",
            //        ClientSecret = "studyAssistConverter_secret",
            //        GrantType = GrantTypes.ClientCredentials,
            //        Scope = "User" // todo

            //    });

            HttpClient dataAccessClient = _httpClientFactory.CreateClient();
            //dataAccessClient.SetBearerToken(tokenResponse.AccessToken);

            Catalog addingCatalog = new()
            {
                Name = catalog.Name,
            };
            
            HttpContent catalogContent = JsonContent.Create(addingCatalog);

            string apiUri = @"http://localhost:5000/api/";
            var response = await dataAccessClient.PostAsync(
                apiUri + "catalogs", catalogContent);

            Catalog? addedCatalog = await response.Content.ReadFromJsonAsync<Catalog>();

            if(addedCatalog != null)
            {

                // здесь можно распараллелить запросы!

                foreach(Theme theme in catalog.Themes)
                {
                    if(theme.Name == null || theme.Name == string.Empty)
                        continue;

                    Theme addingTheme = new Theme()
                    {
                        Name = theme.Name,
                        CatalogId = addedCatalog.CatalogId,
                        ThemeId = null,
                        Issues = null
                    };

                    HttpResponseMessage addingThemeResponseMessage = await dataAccessClient.PostAsync(
                        apiUri + "themes", JsonContent.Create(addingTheme));

                    string prob = await addingThemeResponseMessage.Content.ReadAsStringAsync();

                    Theme? addedTheme = await addingThemeResponseMessage.Content.ReadFromJsonAsync<Theme>();
                    
                    
                    if(addedTheme != null && theme.Issues is not null)
                    {
                        foreach(Issue issue in theme.Issues)
                        {
                            if(issue.Question is null || issue.Question == string.Empty)
                                continue;

                            Issue addingIssue = new()
                            {
                                Question = issue.Question,
                                Answer = issue.Answer,
                                ThemeId = addedTheme.ThemeId,
                            };

                            HttpResponseMessage addingIssueResponseMessage = await dataAccessClient.PostAsync(
                                apiUri + "issues", JsonContent.Create(addingIssue));

                            string possibleErr = await addingIssueResponseMessage.Content.ReadAsStringAsync();

                            Issue? addedIssue = await addingIssueResponseMessage.Content.ReadFromJsonAsync<Issue>();
                        }
                    }
                }
            }

            return 1;
        }

        public void Dispose()
        {
            if(_httpClient != null)
                _httpClient.Dispose();
        }
    }
}
