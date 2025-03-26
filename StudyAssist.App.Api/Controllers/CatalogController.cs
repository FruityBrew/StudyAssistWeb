using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StudyAssist.App.Dtos;
using StudyAssist.Model;
using System.Net.Http.Headers;
using static IdentityModel.OidcConstants;

namespace StudyAssist.App.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IConfiguration _configuration;

        private string? _dataAccessUri;

        public CatalogController(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration) 
        {
            _httpClientFactory = httpClientFactory;
			_configuration = configuration;
		}

        [HttpGet]
        public async Task<ActionResult<List<CatalogsDto>>> GetCatalogs()
        {
            HttpClient dataAccessClient = await _ConfigureAuthenticatedHttpClient();

            _dataAccessUri = _configuration.GetValue<string>("WebApiUrls:KnowledgeDataAccessApi");

            CatalogsDto catalogsDto = await _CreateCatalogsDto(dataAccessClient);

            return Ok(catalogsDto);
        }

        private async Task<CatalogsDto>? _CreateCatalogsDto(HttpClient dataAccessClient)
        {
            CatalogsDto catalogsDto = new();

            List<Catalog> catalogs = await _GetCatalogs(dataAccessClient);

            catalogsDto.Catalogs.AddRange(catalogs.Select(cat => new ItemValue(cat.CatalogId.Value, cat.Name)));

            List<Theme> themes = await _GetThemes(dataAccessClient);

            catalogsDto.Themes.AddRange(themes.Select(t => new ItemValue(t.ThemeId.Value, t.Name, t.CatalogId)));

            List<Issue> issues = await _GetIssues(dataAccessClient);

            catalogsDto.Issues = new(issues.Select(i => new ItemValue(i.IssueId.Value, i.Question, i.ThemeId)));

            return catalogsDto;
        }

        private async Task<List<Catalog>>? _GetCatalogs(HttpClient dataAccessClient)
        {
            HttpResponseMessage response = await dataAccessClient.GetAsync(_dataAccessUri + "catalogs");

            response.EnsureSuccessStatusCode();
            //if(response.IsSuccessStatusCode == false) или так?
            //    return BadRequest(response);

            List<Catalog>? res = await response.Content.ReadFromJsonAsync<List<Catalog>>();

            return res;
        }

        private async Task<List<Theme>>? _GetThemes(HttpClient dataAccessClient)
        {
            HttpResponseMessage response = await dataAccessClient.GetAsync(_dataAccessUri + "themes");

            response.EnsureSuccessStatusCode();
            //if(response.IsSuccessStatusCode == false) или так?
            //    return BadRequest(response);

            List<Theme>? res = await response.Content.ReadFromJsonAsync<List<Theme>>();

            return res;
        }

        private async Task<List<Issue>>? _GetIssues(HttpClient dataAccessClient)
        {
            HttpResponseMessage response = await dataAccessClient.GetAsync(_dataAccessUri + "issues");

            response.EnsureSuccessStatusCode();
            //if(response.IsSuccessStatusCode == false) или так?
            //    return BadRequest(response);

            List<Issue>? res = await response.Content.ReadFromJsonAsync<List<Issue>>();

            return res;
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
