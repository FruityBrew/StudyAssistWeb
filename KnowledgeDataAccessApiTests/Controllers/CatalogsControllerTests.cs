using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KnowledgeDataAccessApi.Controllers;
using KnowledgeDataAccessApi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using StudyAssistModel.DataModel;
using OkResult = Microsoft.AspNetCore.Mvc.OkResult;

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
                    //CatalogId = 1,
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

        [SetUp]
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

        /// <summary>
        /// Arrange: Создаем контроллер с тестовой БД, содержащей данные
        /// Act: Запрашиваем список каталогов со всеми внутренностями.
        /// Arrange: Полученный результат должен совпадать с тем, что есть бд
        /// </summary>
        [Test]
        public async Task GetCatalogs_ValidData_ShouldReturnsAllCatalogsWithInternalCollections()
        {
            CatalogsController codeUnderTest = new (_dbContext);
            var result = await codeUnderTest.GetCatalogs();

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Result, Is.Null);
                Assert.That(result.Value, Is.Not.Null.And.No.Empty);
                Assert.That(result.Value.Count, Is.EqualTo(2));
                Assert.That(result.Value[0].Name, Is.EqualTo("СуперКаталог"));
                Assert.That(
                    result.Value[0].Themes, 
                    Is.Not.Null.And.Not.Empty.And.Count.EqualTo(1));
                Assert.That(
                    result.Value[0].Themes[0].Name, Is.EqualTo("ТемаТем"));
                Assert.That(
                    result.Value[0].Themes[0].Issues, 
                    Is.Not.Null.And.Not.Empty.And.Count.EqualTo(2));
                Assert.That(
                    result.Value[0].Themes[0].Issues[0].Question,
                    Is.EqualTo("Вопрос"));
                Assert.That(
                    result.Value[0].Themes[0].Issues[0].Answer,
                    Is.EqualTo("Ответ"));
            });

        }
    }
}
