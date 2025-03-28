using Microsoft.AspNetCore.Mvc;
using StudyAssist.App.Dtos;
using StudyAssist.Model;

namespace StudyAssist.App.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IssuesController : AppApiControllerBase
    {
        public IssuesController(
            IHttpClientFactory httpClientFactory, IConfiguration configuration) : 
            base(httpClientFactory, configuration)
        {
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IssueDto>> GetIssue(int id)
        {
            HttpClient dataAccessClient = await _ConfigureAuthenticatedHttpClient();

            HttpResponseMessage response = await dataAccessClient.GetAsync(
                _dataAccessUri + "issuesunderstudy/"+ id);

            IssueDto issueDto = new();
            //response.EnsureSuccessStatusCode();
            if(response.IsSuccessStatusCode)
            {
                IssueUnderStudy responsed = await response.Content.ReadFromJsonAsync<IssueUnderStudy>();

                issueDto.AnswerText = responsed.Issue.Answer;
                issueDto.IsStudy = true;
                issueDto.RepeatDate = responsed.RepeateDate;
                issueDto.RepeatCount = responsed.StudyLevel ?? 0;
                issueDto.Id = responsed.Issue.IssueId;
                issueDto.ThemeId = responsed.Issue.ThemeId;

                return Ok(issueDto);
            }
            if(response.StatusCode != System.Net.HttpStatusCode.NotFound)
                response.EnsureSuccessStatusCode();

            HttpResponseMessage issueResponse = await dataAccessClient.GetAsync(
                _dataAccessUri + "issues/" + id);

            issueResponse.EnsureSuccessStatusCode();

            Issue responsedIssue = await issueResponse.Content.ReadFromJsonAsync<Issue>();

            issueDto.AnswerText = responsedIssue.Answer;
            issueDto.ThemeId = responsedIssue.IssueId;
            issueDto.Id = responsedIssue.IssueId;

            return Ok(issueDto);
        }
    }
}
