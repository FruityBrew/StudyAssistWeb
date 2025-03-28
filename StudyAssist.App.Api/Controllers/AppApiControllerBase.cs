using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using static IdentityModel.OidcConstants;

namespace StudyAssist.App.Api.Controllers
{
    public class AppApiControllerBase : ControllerBase
    {
        protected readonly IHttpClientFactory _httpClientFactory;
        protected readonly IConfiguration _configuration;

        protected readonly string? _dataAccessUri;

        public AppApiControllerBase(
            IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _dataAccessUri = _configuration.GetValue<string>("WebApiUrls:KnowledgeDataAccessApi");
        }


        protected async Task<HttpClient> _ConfigureAuthenticatedHttpClient()
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
