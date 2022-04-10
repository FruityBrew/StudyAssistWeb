using KnowledgeDataAccessApi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyAssistModel.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        /// Получить все каталоги.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<Catalog>>> Get()
        {
            return await DoAsync(async () => 
            {
                List<Catalog> catalogs = await _dbContext.Catalogs.ToListAsync();
                return await ToAsyncResult(catalogs);
            });
        }

        /// <summary>
        /// Получить все темы каталога
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/themes")]
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// Добавить пустой каталог
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        /// <summary>
        /// Изменяет имя каталога
        /// </summary>
        /// <param name="name"></param>
        [HttpPatch("{name}")]
        public void Patch(string name)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        #endregion Api
    }
}
