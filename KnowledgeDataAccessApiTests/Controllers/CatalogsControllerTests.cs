using KnowledgeDataAccessApi.Controllers;
using KnowledgeDataAccessApi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using StudyAssistModel.DataModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnowledgeDataAccessApiTests.Controllers
{
    [TestFixture]
    public class CatalogsControllerTests
    {
        private KnowledgeContext _dbContext;

        public async Task GenerateDbData()
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

        }

        /// <summary>
        /// Arrange:
        /// Act: 
        /// Arrange: 
        /// </summary>

        /// <summary>
        /// Arrange: Создаем контроллер с тестовой БД, содержащей данные
        /// Act: Запрашиваем список каталогов.
        /// Arrange: Полученный результат должен совпадать с тем, что есть бд
        /// </summary>
        [Test]
        public async Task GetCatalogs_ValidData_ShouldReturnsAllCatalogs()
        {
            await GenerateDbData();
            CatalogsController codeUnderTest = new (_dbContext);

            var result = await codeUnderTest.GetCatalogs();

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Result, Is.Null);
                Assert.That(result.Value, Is.Not.Null.And.No.Empty);
                Assert.That(result.Value.Count, Is.EqualTo(2));
                Assert.That(result.Value[0].Name, Is.EqualTo("СуперКаталог"));
                Assert.That(result.Value[1].Name, Is.EqualTo("Идемп"));

                //Assert.That(
                //    result.Value[0].Themes, 
                //    Is.Not.Null.And.Not.Empty.And.Count.EqualTo(1));
                //Assert.That(
                //    result.Value[0].Themes[0].Name, Is.EqualTo("ТемаТем"));
                //Assert.That(
                //    result.Value[0].Themes[0].Issues, 
                //    Is.Not.Null.And.Not.Empty.And.Count.EqualTo(2));
                //Assert.That(
                //    result.Value[0].Themes[0].Issues[0].Question,
                //    Is.EqualTo("Вопрос"));
                //Assert.That(
                //    result.Value[0].Themes[0].Issues[0].Answer,
                //    Is.EqualTo("Ответ"));
            });
        }

        /// <summary>
        /// Arrange: Не заполняем бд, создаем контроллер с пустой бд
        /// Act: Вызываем запрос каталогов
        /// Arrange: Ок резалт с пустой коллекцией
        /// </summary>
        [Test]
        public async Task GetCatalogs_EmptyDb_ShouldReturnsEmptyResult()
        {
            CatalogsController codeUnderTest = new(_dbContext);

            var result = await codeUnderTest.GetCatalogs();

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Result, Is.Null);
                Assert.That(result.Value, Is.Not.Null.And.Empty);
            });
        }

        /// <summary>
        /// Arrange: Создаем контроллер с тестовой БД, содержащей данные
        /// Act: Запрашиваем каталогов по идентификатору
        /// Arrange: Полученный результат должен совпадать с тем, что есть бд
        /// </summary>
        [Test]
        public async Task GetCatalog_ValidData_ShouldReturnsCatalog()
        {
            await GenerateDbData();
            CatalogsController codeUnderTest = new(_dbContext);

            var result = await codeUnderTest.GetCatalog(1);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Result, Is.Null);
                Assert.That(result.Value.Name, Is.EqualTo("СуперКаталог"));
            });
        }

        /// <summary>
        /// Arrange: Создаем контроллер с тестовой БД, содержащей данные
        /// Act: Запрашиваем каталогов по идентификатору, не существующего в бд
        /// Arrange: Not found result
        /// </summary>
        [Test]
        public async Task GetCatalog_NotExistsCatalogId_ShouldReturnsNotFoundResult()
        {
            await GenerateDbData();
            CatalogsController codeUnderTest = new(_dbContext);

            var result = await codeUnderTest.GetCatalog(10);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Result, Is.Not.Null);
                Assert.That(result.Result, Is.InstanceOf(typeof(NotFoundResult)));
            });
        }

        /// <summary>
        /// Arrange: Создаем контроллер с тестовой БД, содержащей данные
        /// Act: Запрашиваем список тем каталога
        /// Arrange: Должен быть возвращен список тем с валидными данными
        /// </summary>
        [Test]
        public async Task GetCatalogThemes_RegularWorkFlow_ShouldReturnsThemesList()
        {
            await GenerateDbData();
            _dbContext.Themes.Add(new Theme
            {
                CatalogId = 1,
                Name = "C++"
            });
            CatalogsController codeUnderTest = new(_dbContext);

            var result = await codeUnderTest.GetCatalogThemes(1);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Result, Is.Null);
                Assert.That(result.Value, Is.Not.Null.And.Count.EqualTo(2));
                Assert.That(result.Value[0].Name, Is.EqualTo("ТемаТем"));
                Assert.That(result.Value[1].Name, Is.EqualTo("C++"));
            });
        }

        /// <summary>
        /// Arrange: Создаем контроллер с тестовой БД, содержащей данные
        /// Act: Запрашиваем список тем несуществующего в бд каталога 
        /// Arrange: Not found result
        /// </summary>
        [Test]
        public async Task GetCatalogThemes_NotExistsCatalogId_ShouldReturnsNotFoundResult()
        {
            await GenerateDbData();
            CatalogsController codeUnderTest = new(_dbContext);

            var result = await codeUnderTest.GetCatalogThemes(100);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Result, Is.Not.Null);
                Assert.That(result.Result, Is.InstanceOf(typeof(NotFoundResult)));
            });
        }

        /// <summary>
        /// Arrange: Создаем контроллер с пустой БД. Создаем добавлемую сущность
        /// Act: вызываем добавление новой сущности
        /// Arrange: в бд дожна появиться запись и должен вернуться
        /// CreatedAtActionResult с добавленным каталогом и ссылкой на ресурс
        /// с добавленным каталогом
        /// </summary>
        [Test]
        public async Task AddCatalog_RegularWorkFlow_ShoulReturnsAddedItem()
        {
            CatalogsController codeUnderTest = new(_dbContext);
            Catalog addedItem = new ()
            {
                Name = "ДжаваСкрипт"
            };

            var result = await codeUnderTest.AddCatalog(addedItem);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Value, Is.Null);
                Assert.That(result.Result, Is.InstanceOf(typeof(CreatedAtActionResult)));
            });

            var createdAtActionResult = result.Result as CreatedAtActionResult;

            Assert.Multiple(() =>
            {
                Assert.That(createdAtActionResult, Is.Not.Null);
                Assert.That(
                    createdAtActionResult.Value, 
                    Is.Not.Null.And.InstanceOf(typeof(Catalog)));

                Catalog addedItem = createdAtActionResult.Value as Catalog;

                Assert.That(addedItem.Name, Is.EqualTo("ДжаваСкрипт"));
                Assert.That(addedItem.CatalogId, Is.EqualTo(1));

                Assert.That(
                    createdAtActionResult.ActionName, 
                    Is.EqualTo("GetCatalog"));
                Assert.That(
                    createdAtActionResult.RouteValues.TryGetValue(
                        "id", out object result), 
                    Is.True);

                createdAtActionResult.RouteValues.TryGetValue(
                    "id", out object routValueResult);
                Assert.That(routValueResult.ToString(), Is.EqualTo("1"));
            });

        }
    }
}
