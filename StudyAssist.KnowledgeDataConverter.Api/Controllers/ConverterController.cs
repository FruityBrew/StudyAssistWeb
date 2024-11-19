using Microsoft.AspNetCore.Mvc;

namespace StudyAssist.KnowledgeDataConverter.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ConverterController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> Convert([FromBody] Input input)
        {

            await FromFileToDbConvertor.Convert();

            return await Task.FromResult(Ok(input.Path));
        }

        public class Input
        {
            public string Path { get; set; }
        }
    }
}
