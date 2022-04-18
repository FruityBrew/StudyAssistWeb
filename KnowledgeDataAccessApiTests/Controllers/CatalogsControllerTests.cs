using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KnowledgeDataAccessApi.Controllers;
using KnowledgeDataAccessApi.Model;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using StudyAssistModel.DataModel;

namespace KnowledgeDataAccessApiTests.Controllers
{
    [TestFixture]
    public class CatalogsControllerTests
    {
        private KnowledgeContext _dbContext;

        private async Task _GenerateDbData()
        {
            List<Catalog> catalogs = new() 
            {
                new Catalog
                {
                    CatalogId = 1,
                    Name = "СуперКаталог",
                    Themes = new ()
                    {
                        new Theme()
                        {
                            Name = "ТемаТем",
                            Issues = new()
                            {
                                new Issue()
                                {
                                    Answer = "Ответ",
                                    Question = "Вопрос"
                                },
                                new Issue()
                                {
                                    Answer = "ОтветЩ",
                                    Question = "ВопросНН"
                                }
                            }
                        }
                    }
                },
                new Catalog
                {
                    Name = "Идемп"
                }
            };

            await _dbContext.Catalogs.AddRangeAsync(catalogs);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Setup()
        {
            var dbOptions = new DbContextOptionsBuilder<KnowledgeContext>()
                .UseInMemoryDatabase(databaseName: "Knowledge DataBase")
                .Options;

            _dbContext = new KnowledgeContext(dbOptions);
            await _dbContext.Database.EnsureDeletedAsync();
            await _dbContext.Database.EnsureCreatedAsync();

            await _GenerateDbData();
        }

        [Test]
        public async Task GetCatalogs_ValidData_CorrectResult()
        {
            CatalogsController codeUnderTest = new (_dbContext);
            var result = await codeUnderTest.GetCatalogs();

        }
    }
}
