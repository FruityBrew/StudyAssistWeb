using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KnowledgeDataAccessApi.Model;
using StudyAssistModel.DataModel;
using Utilities;
using Microsoft.EntityFrameworkCore;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KnowledgeDataAccessApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IssuesController : DbContextController<KnowledgeContext>
    {

        #region Constructors

        public IssuesController(KnowledgeContext knowledgeDbContext) 
            : base(knowledgeDbContext)
        {
        }

        #endregion Constructors

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Issue>>> Get()
        {
            return await DoAsync(async () =>
            {
                var result = await _dbContext.Issues.ToListAsync();

                return await ToAsyncResult<IEnumerable<Issue>>(result);
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Issue>> GetAsync(int id)
        {
            return await DoAsync(async () =>
            {
                var result = await _dbContext.Issues.FirstOrDefaultAsync(
                    item => item.IssueId == id);

                return await ToAsyncResult(result);
            });
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Issue item)
        {
            return await DoAsync(async () =>
            {
                _dbContext.Issues.Add(item);
                await _dbContext.SaveChangesAsync();

                return await Task.FromResult(Ok());
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Issue value)
        {
            return await DoAsync(async () =>
            {
                Issue updatedItem = await _dbContext.Issues.FirstOrDefaultAsync(
                    item => item.IssueId == id);

                updatedItem.Answer = value.Answer;
                updatedItem.Question = value.Question;
                
                await _dbContext.SaveChangesAsync();

                return await Task.FromResult(Ok());
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await DoAsync(async () =>
            {
                Issue deletedItem = await _dbContext.Issues.FirstOrDefaultAsync(
                    item => item.IssueId == id);

                if (deletedItem == null)
                    return ToError("Запись не найдена в БД", false);

                _dbContext.Issues.Remove(deletedItem);

                return await Task.FromResult(Ok());
            });
        }
    }
}
