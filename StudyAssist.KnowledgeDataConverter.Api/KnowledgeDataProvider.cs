using IdentityModel.Client;
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
    internal class KnowledgeDataProvider : IKnowledgeDataProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;
        //private readonly IConfiguration _configuration;

        private string _identityServerUri = @"AuthConfig:IdentityServerAuthorityUrl";

        private string _dataAccessUri = @"KnowledgeDataAccessApi";

        public KnowledgeDataProvider(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
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
            catalog.Themes = null;
            // разобраться почему с темами не проходит
            HttpContent content = JsonContent.Create(catalog, typeof(Catalog));

            string uri = @"http://localhost:5000/api/catalogs";
            var response = await dataAccessClient.PostAsync(
                uri, content);

            return 1;
        }
    }
}
