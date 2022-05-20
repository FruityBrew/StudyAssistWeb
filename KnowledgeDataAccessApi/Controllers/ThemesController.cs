using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KnowledgeDataAccessApi.Model;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using StudyAssistModel.DataModel;
using Utilities;

namespace KnowledgeDataAccessApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThemesController : DbContextController<KnowledgeContext>
    {
        #region Constructor

        public ThemesController(KnowledgeContext context) : base(context)
        {
        }

        #endregion Constructor

        #region Api

        /// <summary>
        /// Запрашивает все задачи темы
        /// </summary>
        /// <param name="id">Идентификатор темы</param>
        [HttpGet("{id}/issues")]
        public async Task<ActionResult<List<Issue>>> GetThemeIssues(int id)
        {
            Theme targetTheme = await _dbContext.Themes
                .FirstOrDefaultAsync(item => item.ThemeId == id);

            if (targetTheme == null)
                return NotFound();

            return targetTheme.Issues ?? Enumerable.Empty<Issue>().ToList();
        }

        /// <summary>
        /// Запрашивает тему.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Theme>> GetTheme(int id)
        {
            Theme targetTheme = await _dbContext.Themes
                .FirstOrDefaultAsync(item => item.ThemeId == id);

            return targetTheme;
        }

        /// <summary>
        /// Запрашивает добавление темы
        /// </summary>
        /// <param name="addedItem">Объект с добавляемой темой</param>
        [HttpPost]
        public async Task<ActionResult<Theme>> AddTheme([FromBody] Theme addedItem)
        {
            var addedEntity = await _dbContext.Themes.AddAsync(
                new Theme()
                {
                    CatalogId = addedItem.CatalogId,
                    Name = addedItem.Name
                });

            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetTheme),
                new {id = addedEntity.Entity.ThemeId},
                addedEntity.Entity);
        }

        /// <summary>
        /// Запрашивает обновление имени темы
        /// </summary>
        /// <param name="id">Идентификатор обновляемой темы</param>
        /// <param name="updatedItem">Инструкции обновления</param>
        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateThemeName(
            int id, [FromBody] JsonPatchDocument<Theme> updatedItem)
        {
            Theme targetTheme = await _dbContext.Themes
                .FirstOrDefaultAsync(item => item.ThemeId == id);

            if (targetTheme == null)
                return NotFound();

            updatedItem.ApplyTo(targetTheme);

            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Запрашивает удаление темы
        /// </summary>
        /// <param name="id">Идентификатор удаляемой темы</param>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTheme(int id)
        {
            Theme targetTheme = await _dbContext.Themes
                .FirstOrDefaultAsync(item => item.ThemeId == id);

            if (targetTheme == null)
                return NotFound();

            _dbContext.Themes.Remove(targetTheme);

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        #endregion Api
    }
}
