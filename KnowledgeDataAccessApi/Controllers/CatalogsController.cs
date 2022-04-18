using KnowledgeDataAccessApi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyAssistModel.DataModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KnowledgeDataAccessApi.Constants;
using Microsoft.AspNetCore.JsonPatch;
using Utilities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        public async Task<ActionResult<List<Catalog>>> GetCatalogs()
        {
            return await _dbContext.Catalogs.ToListAsync();
        }

        /// <summary>
        /// Запрашивает каталог.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Catalog>> GetCatalog(int id)
        {
            Catalog targetCatalog = await _dbContext.Catalogs
                // может понадобиться:
                //.Include(catalog => catalog.Themes)
                //.ThenInclude(theme => theme.Issues)
                .FirstOrDefaultAsync(item => item.CatalogId == id);

            return targetCatalog;
        }

        /// <summary>
        /// Запрашивает все темы каталога
        /// </summary>
        /// <param name="id">Идентификатор каталога</param>
        [HttpGet("{id}/themes")]
        public async Task<ActionResult<IEnumerable<Theme>>> GetCatalogThemes(int id)
        {
            Catalog targetCatalog = await _dbContext.Catalogs
                .FirstOrDefaultAsync(item => item.CatalogId == id);

            if (targetCatalog == null)
                return NotFound();

            return targetCatalog.Themes;
        }

        /// <summary>
        /// Запрашивает добавление пустого каталога
        /// </summary>
        /// <param name="value">Каталог</param>
        [HttpPost]
        public async Task<ActionResult<Catalog>> AddCatalog(
            [FromBody] Catalog addedItem)
        {
            var addedEntity = await _dbContext.Catalogs.AddAsync(
                new Catalog
                {
                    Name = addedItem.Name
                });

            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetCatalog),
                new {id = addedEntity.Entity.CatalogId},
                addedEntity.Entity);
        }

        /// <summary>
        /// Изменяет имя каталога
        /// </summary>
        /// <param name="name"></param>
        [HttpPatch("{id}")]
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

        [HttpDelete("{id}")]
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
