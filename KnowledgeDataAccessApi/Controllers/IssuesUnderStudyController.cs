using KnowledgeDataAccessApi.Model;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyAssistModel.DataModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities;

namespace KnowledgeDataAccessApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssuesUnderStudyController : DbContextController<KnowledgeContext>
    {
        #region Constructor

        public IssuesUnderStudyController(KnowledgeContext dbContext)
            : base(dbContext)
        { }

        #endregion Constructor

        #region Api

        /// <summary>
        /// Запрашивает список вопросов на изучении
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IssueUnderStudy>>> GetIssuesUnderStudy()
        {
            return await _dbContext.IssuesUnderStudy.ToListAsync();
        }

        /// <summary>
        /// Запрашивает вопрос на обученни по идентификатору
        /// </summary>
        /// <param name="id">Идентфикатор вопроса на изучении</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<IssueUnderStudy>> GetIssueUnderStudy(int id)
        {
            return await _dbContext.IssuesUnderStudy
                .Include(issueUnder => issueUnder.Issue)
                .ThenInclude(issue => issue.Theme)
                .ThenInclude(theme => theme.Catalog)
                .FirstOrDefaultAsync(dbItem => dbItem.IssueUnderStudyId == id);
        }

        /// <summary>
        /// Запрашивает добавление новой задачи на изучении.
        /// </summary>
        /// <param name="addedItem">Добавляемый элемент</param>
        [HttpPost]
        public async Task<ActionResult<IssueUnderStudy>> AddIssueUnderStudy(
            [FromBody] IssueUnderStudy addedItem)
        {
            var addedEntity = await _dbContext.IssuesUnderStudy.AddAsync(addedItem);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetIssueUnderStudy),
                new {id = addedEntity.Entity.IssueUnderStudyId},
                addedEntity.Entity);
        }

        /// <summary>
        /// Запрашивает обновление задачи на изучении 
        /// </summary>
        /// <param name="id">Идентификатор обновляемой задачи</param>
        /// <param name="updatedPatch">Патч обновления</param>
        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateIssueUnderStudy(
            int id, [FromBody] JsonPatchDocument<IssueUnderStudy> updatedPatch)
        {
            IssueUnderStudy targetItem = await _dbContext.IssuesUnderStudy
                .FirstOrDefaultAsync(dbItem => dbItem.IssueUnderStudyId == id);

            if (targetItem == null)
                return NotFound($"IssueUnderStudy with id = {id} not found");

            updatedPatch.ApplyTo(targetItem);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Запрашивает удаление задачи на изучении.
        /// </summary>
        /// <param name="id">Идентификатор удаляемого элемента</param>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteIssueUnderStudy(int id)
        {
            IssueUnderStudy targetItem = await _dbContext.IssuesUnderStudy
                .FirstOrDefaultAsync(dbItem => dbItem.IssueUnderStudyId == id);

            if (targetItem == null)
                return NotFound($"IssueUnderStudy with id = {id} not found");

            _dbContext.IssuesUnderStudy.Remove(targetItem);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        #endregion Api
    }
}
