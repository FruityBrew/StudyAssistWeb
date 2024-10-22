using KnowledgeDataAccessApi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KnowledgeDataAccessApi.Constants;
using Microsoft.AspNetCore.JsonPatch;
using Utilities;
using Microsoft.AspNetCore.Authorization;
using StudyAssist.Model;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
#nullable enable

namespace KnowledgeDataAccessApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogsController : DbContextController<KnowledgeContext>
    {
        #region Constructor

        public CatalogsController(KnowledgeContext context) : base(context)
        {
        }

        #endregion Constructor

        #region Api

        /// <summary>
        /// Запрашивает все каталоги.
        /// </summary>
        [HttpGet]
		[Authorize("KnowledgeApi.Read")]
        public async Task<ActionResult<List<Catalog>>> GetCatalogs()
        {
            return await _dbContext.Catalogs.ToListAsync();
        }

        /// <summary>
        /// Запрашивает каталог.
        /// </summary>
        [HttpGet("{id}")]
        [Authorize("KnowledgeApi.Read")]
        public async Task<ActionResult<Catalog>> GetCatalog(int id)
        {
            Catalog targetCatalog = await _dbContext.Catalogs
                // может понадобиться:
                //.Include(catalog => catalog.Themes)
                //.ThenInclude(theme => theme.Issues)
                .FirstOrDefaultAsync(item => item.CatalogId == id);

            if(targetCatalog == null) 
                return NotFound();

            return targetCatalog;
        }

        /// <summary>
        /// Запрашивает все темы каталога
        /// </summary>
        /// <param name="id">Идентификатор каталога</param>
        [HttpGet("{id}/themes")]
		[Authorize("KnowledgeApi.Read")]
        public async Task<ActionResult<List<Theme>>> GetCatalogThemes(int id)
        {
            Catalog targetCatalog = await _dbContext.Catalogs
                .FirstOrDefaultAsync(item => item.CatalogId == id);

            if (targetCatalog == null)
                return NotFound();

            return targetCatalog.Themes ?? Enumerable.Empty<Theme>().ToList();
        }

        /// <summary>
        /// Запрашивает добавление пустого каталога
        /// </summary>
        /// <param name="value">Каталог</param>
        [HttpPost]
        [Authorize("KnowledgeApi.Write")]
		public async Task<ActionResult<Catalog>> AddCatalog(
            [FromBody] Catalog addedItem)
        {
            var addedEntity = await _dbContext.Catalogs.AddAsync(addedItem);

            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetCatalog),
                new {id = addedEntity.Entity.CatalogId},
                addedEntity.Entity);
        }

        /// <summary>
        /// Изменяет имя каталога
        /// </summary>
        [HttpPatch("{id}")]
		[Authorize("KnowledgeApi.Write")]
		public async Task<ActionResult> UpdateCatalogName(
            int id, [FromBody] JsonPatchDocument<Catalog> updatedItem)
        {
            Catalog targetCatalog = await _dbContext.Catalogs
                .FirstOrDefaultAsync(item => item.CatalogId == id);

            if (targetCatalog == null)
                return NotFound();

            updatedItem.ApplyTo(targetCatalog);

            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Запрашивает удаление каталога
        /// </summary>
        [HttpDelete("{id}")]
		[Authorize("KnowledgeApi.Write")]
		public async Task<ActionResult> DeleteCatalog(int id)
        {
            Catalog deletedCatalog = await _dbContext.Catalogs
                .FirstOrDefaultAsync(item => item.CatalogId == id);

            if (deletedCatalog == null)
                return NotFound();

            _dbContext.Catalogs.Remove(deletedCatalog);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        #endregion Api
    }
}
