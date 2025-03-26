using IdentityModel.Client;
using StudyAssist.App.Dtos;
using StudyAssist.BlazorApp.Interfaces;
using StudyAssist.BlazorApp.ViewModels;
using static IdentityModel.OidcConstants;

namespace StudyAssist.BlazorApp.Services
{
    internal class AppDtoReceivedService : IDtoReceiverService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public AppDtoReceivedService(
            IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<List<CatalogVm>> GetCatalogTreeAsync()
        {
            HttpClient dataAccessClient = await _ConfigureAuthenticatedHttpClient();

            string catalogsUrl = _configuration.GetValue<string>("WebApiUrls:AppApi");

            HttpResponseMessage response = await dataAccessClient.GetAsync(catalogsUrl + "catalog/getcatalogs");

            response.EnsureSuccessStatusCode();

            String catalogsDtoStr = await response.Content.ReadAsStringAsync();

            CatalogsDto catalogsDto = await response.Content.ReadFromJsonAsync<CatalogsDto>();

            List<CatalogVm> catalogs = catalogsDto.Catalogs
                .Select(catalog => new CatalogVm
                {
                    Id = catalog.Id,
                    Name = catalog.Name,
                    Themes = catalogsDto.Themes
                        .Where(theme => theme.ParentId == catalog.Id)
                        .Select(theme => new ThemeVm
                        {
                            Id = theme.Id,
                            Name = theme.Name,
                            ParentId = catalog.Id,
                            Issues = catalogsDto.Issues
                                .Where(issue => issue.ParentId == theme.Id)
                                .Select(issue => new ItemVm()
                                {
                                    Id = issue.Id,
                                    Name = issue.Name,
                                    ParentId = theme.Id
                                }).ToList()
                        }).ToList(),
                })
                .ToList();

            return catalogs;
        }

        public Task<EditingIssueVm> GetEditingIssueAsync()
        {
            throw new NotImplementedException();
        }


        private async Task<HttpClient> _ConfigureAuthenticatedHttpClient()
        {
            string? identityServerUri = _configuration.GetValue<string>("AuthConfig:IdentityServerAuthorityUrl");
            HttpClient identityServerClient = _httpClientFactory.CreateClient();

            DiscoveryDocumentResponse discoveryDocument = await identityServerClient.GetDiscoveryDocumentAsync(
                identityServerUri);

            var tokenResponse = await identityServerClient.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest
                {
                    Address = discoveryDocument.TokenEndpoint,
                    ClientId = "studyAssist_id",
                    ClientSecret = "studyAssist_secret",
                    GrantType = GrantTypes.ClientCredentials,
                    Scope = "User"

                });

            HttpClient client = _httpClientFactory.CreateClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            return client;
        }
    }


}
