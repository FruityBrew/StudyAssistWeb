using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StudyAssist.Model;
using System.Net.Http.Headers;
using static IdentityModel.OidcConstants;

namespace StudyAssist.App.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IConfiguration _configuration;

		public TestController(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration) 
        {
            _httpClientFactory = httpClientFactory;
			_configuration = configuration;
		}

        [HttpGet]
        public async Task<ActionResult<List<Catalog>>> GetCatalogs()
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
            string? dataAccessUri = _configuration.GetValue<string>("WebApiUrls:KnowledgeDataAccessApi");

            HttpClient dataAccessClient = _httpClientFactory.CreateClient();
            dataAccessClient.SetBearerToken(tokenResponse.AccessToken);


            var response = await dataAccessClient.GetAsync(dataAccessUri + "catalogs");

            response.EnsureSuccessStatusCode(); 
            //if(response.IsSuccessStatusCode == false) или так?
            //    return BadRequest(response);

            List<Catalog>? res = await response.Content.ReadFromJsonAsync<List<Catalog>>();

            return Ok(res);
        }
    }
}
