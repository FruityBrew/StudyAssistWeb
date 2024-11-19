using Microsoft.AspNetCore.Mvc;

namespace StudyAssist.KnowledgeDataConverter.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ConverterController : ControllerBase
    {
        private readonly IKnowledgeDataConverter _dataConverter;

        public ConverterController(IKnowledgeDataConverter converter) 
        { 
            _dataConverter = converter;
        }

        [HttpPost]
        public async Task<ActionResult> Convert([FromBody] Input input)
        {

            await _dataConverter.Convert();

            return await Task.FromResult(Ok(input.Path));
        }

        public class Input
        {
            public string Path { get; set; }
        }
    }
}
