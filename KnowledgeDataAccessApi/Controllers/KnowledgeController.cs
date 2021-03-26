using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KnowledgeDataAccessApi.Model;
using Microsoft.AspNetCore.Mvc;
using StudyAssistModel.DataModel;

namespace KnowledgeDataAccessApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class KnowledgeController : ControllerBase
    {
        //public async Task<Catalog> GetAllIssues()
        //{

        //}

        private KnowledgeContext _context;
        public KnowledgeController(KnowledgeContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> CreateDb()
        {

                _context.Catalogs.Add(new Catalog { Name = "Test" });
                await _context.SaveChangesAsync();

            return await Task.FromResult(new OkResult());
        }
    }
}
