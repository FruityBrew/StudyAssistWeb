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

        public Task<int> AddCatalogAsync(CatalogVm catalog)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddIssueAsync(EditingIssueVm issue)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddThemeAsync(ThemeVm theme)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCatalogAsync(CatalogVm source)
        {
            throw new NotImplementedException();
        }

        public Task DeleteIssueAsync(ItemVm deleted)
        {
            throw new NotImplementedException();
        }

        public Task DeleteThemeAsync(ThemeVm source)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CatalogVm>> GetCatalogTreeAsync()
        {
            CatalogsDto catalogsDto = await _RequestAsync<CatalogsDto>("catalog/getcatalogs");

            List <CatalogVm> catalogs = catalogsDto.Catalogs
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

        public async Task<EditingIssueVm> GetEditingIssueAsync(int issueId)
        {
            IssueDto issueDto = await _RequestAsync<IssueDto>("issues/getissue/"+issueId);

            EditingIssueVm issueVm = new()
            {
                AnswerText = issueDto.AnswerText,
                RepeatCount = issueDto.RepeatCount,
                Id = issueDto.Id.Value,
                IsStudy = issueDto.IsStudy,
                ParentId = issueDto.ThemeId.Value,
                RepeatDate = issueDto.RepeatDate,
            };

            return issueVm;
        }

        private async Task<T> _RequestAsync<T>(string urlSegment) where T : class
        {
            HttpClient dataAccessClient = await _ConfigureAuthenticatedHttpClient();
            string catalogsUrl = _configuration.GetValue<string>("WebApiUrls:AppApi");
            HttpResponseMessage response = await dataAccessClient.GetAsync(
                catalogsUrl + urlSegment);
            response.EnsureSuccessStatusCode();

            T responsed = await response.Content.ReadFromJsonAsync<T>();

            return responsed;
        }

        public Task<List<CatalogVm>> GetRepeatCatalogTreeAsync(DateTime date)
        {
            throw new NotImplementedException();
        }

        public Task ProlongIssueStudyDateAsync(EditingIssueVm issueVm)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCatalogNameAsync(CatalogVm source)
        {
            throw new NotImplementedException();
        }

        public Task UpdateIssueAnswerAsync(EditingIssueVm issueVm)
        {
            throw new NotImplementedException();
        }

        public Task UpdateIssueNameAsync(ItemVm source)
        {
            throw new NotImplementedException();
        }

        public Task UpdateIssueStudyAsync(EditingIssueVm issueVm)
        {
            throw new NotImplementedException();
        }

        public Task UpdateThemeNameAsync(ThemeVm source)
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
