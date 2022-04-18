using KnowledgeDataAccessApi.Model;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyAssistModel.DataModel;
using System.Threading.Tasks;
using Utilities;

namespace KnowledgeDataAccessApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssuesController : DbContextController<KnowledgeContext>
    {
        #region Constructors

        public IssuesController(KnowledgeContext knowledgeDbContext) 
            : base(knowledgeDbContext)
        {
        }

        #endregion Constructors

        #region Api
        
        /// <summary>
        /// Запрашивает вопрос
        /// </summary>
        /// <param name="id">Идентификатор вопроса</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<Issue>> GetIssue(int id)
        {
            return await _dbContext.Issues.FirstOrDefaultAsync(
                item => item.IssueId == id);
        }

        /// <summary>
        /// Запрашивает добавление нового вопроса.
        /// </summary>
        /// <param name="addedItem">Добавляемый вопрос</param>
        [HttpPost]
        public async Task<ActionResult> AddIssue([FromBody] Issue addedItem)
        {
            var addedEntity = await _dbContext.Issues.AddAsync(addedItem);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetIssue),
                new {id = addedEntity.Entity.IssueId},
                addedEntity.Entity);
        }

        /// <summary>
        /// Запрашивает обновление данных задачи.
        /// </summary>
        /// <param name="id">Идентификатор обновляемой задачи</param>
        /// <param name="updatedItem">Патч обвновления</param>
        [HttpPatch("{id}")]
        public async Task<ActionResult> Put(
            int id, [FromBody] JsonPatchDocument<Issue> updatedItem)
        {
            Issue targetItem = await _dbContext.Issues
                .FirstOrDefaultAsync(item => item.IssueId == id);

            if(targetItem == null)
                return NotFound();

            updatedItem.ApplyTo(targetItem);

            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Запрашивает удаление задачи.
        /// </summary>
        /// <param name="id">Идентификатор задачи, подлежащей удалению</param>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteIssue(int id)
        {
            Issue deletedItem = await _dbContext.Issues.FirstOrDefaultAsync(
                item => item.IssueId == id);

            if (deletedItem == null)
                return NotFound();

            _dbContext.Issues.Remove(deletedItem);

            return NoContent();
        }

        #endregion Api
    }
}
