using KnowledgeDataAccessApi.Controllers;
using KnowledgeDataAccessApiTests.Utilities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using StudyAssistModel.DataModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnowledgeDataAccessApiTests.Controllers
{
    [TestFixture]
    public class CatalogsControllerTests : TestsBase
    {
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
        /// Arrange: В бд есть каталог без тем
        /// Act: Запрашиваем список тем по идентификатору каталога, у которого нет тем.
        /// Arrange: Ок резалт с пустой коллекцией
        /// </summary>
        [Test]
        public async Task GetCatalogs_NoThemeInCatalog_ShouldReturnsEmptyResult()
        {
            await GenerateDbData();

            CatalogsController codeUnderTest = new(_dbContext);

            var result = await codeUnderTest.GetCatalogThemes(2);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Result, Is.Null);
                Assert.That(result.Value, Is.Not.Null.And.Empty);
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
        /// Arrange: должен вернуться CreatedAtActionResult с добавленным каталогом и ссылкой на ресурс
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
                var createdAtActionResult = result.Result as CreatedAtActionResult;
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
                        "id", out object routValueResult),
                    Is.True);
                Assert.That(routValueResult.ToString(), Is.EqualTo("1"));
            });
        }

        /// <summary>
        /// Arrange: Создаем контроллер с пустой БД. Создаем добавлемую сущность
        /// Act: вызываем добавление новой сущности
        /// Arrange: в бд дожна появиться запись с добавленным каталогом
        /// </summary>
        [Test]
        public async Task AddCatalog_RegularWorkFlow_ShouldBeAddedItemInDb()
        {
            CatalogsController codeUnderTest = new(_dbContext);
            Catalog addedItem = new()
            {
                Name = "ДжаваСкрипт"
            };

            var result = await codeUnderTest.AddCatalog(addedItem);
            var addedItemRes = await codeUnderTest.GetCatalog(1);

            Assert.Multiple(() =>
            {
                Assert.That(addedItemRes, Is.Not.Null);
                Assert.That(addedItemRes.Result, Is.Null);
                Assert.That(addedItemRes.Value.Name, Is.EqualTo("ДжаваСкрипт"));
            });
        }

        /// <summary>
        /// Arrange: Создаем базу с данными, контроллер, патч обновления имени каталога
        /// Act: Вызываем обновление имени существующего каталога.
        /// Assert: Должен вернуться OK
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task UpdateCatalog_RegularWorkFlow_ShuldReturnsOk()
        {
            await GenerateDbData();
            CatalogsController codeUnderTest = new (_dbContext);

            JsonPatchDocument<Catalog> patch = new JsonPatchDocument<Catalog>(
                new List<Operation<Catalog>>
                {
                    new ("replace", "name", string.Empty, "newName"),
                }, 
                new DefaultContractResolver());

            var result = await codeUnderTest.UpdateCatalogName(1, patch);

            Assert.That(result, Is.InstanceOf<OkResult>());
        }

        /// <summary>
        /// Arrange: Создаем базу с данными, контроллер, патч обновления имени каталога
        /// Act: Вызываем обновление имени НЕ существующего каталога.
        /// Assert: Должен вернуться NotFoundResult
        /// </summary>
        [Test]
        public async Task UpdateCatalogName_NotExistsCatalogId_ShouldReturnsNotFound()
        {
            await GenerateDbData();
            CatalogsController codeUnderTest = new(_dbContext);

            JsonPatchDocument<Catalog> patch = new JsonPatchDocument<Catalog>(
                new List<Operation<Catalog>>
                {
                    new ("replace", "name", string.Empty, "newName"),
                },
                new DefaultContractResolver());

            var result = await codeUnderTest.UpdateCatalogName(100, patch);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        /// <summary>
        /// Arange: Создаем базу с данными, контроллер
        /// Act: Запрашиваем удаление каталога
        /// Assert: Ожидаем НоуКонтентРезалт и отсутсвие в бд сущностей, связанных
        /// с удаляемым каталогом.
        /// </summary>
        [Test]
        public async Task DeleteCatalog_RegularWorkFlow_ShouldReturnsNoContent()
        {
            await GenerateDbData();
            CatalogsController codeUnderTest = new(_dbContext);
            var result = await codeUnderTest.DeleteCatalog(1);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<NoContentResult>());
                Assert.That(
                    _dbContext.Catalogs.Where(cat => cat.CatalogId == 1), 
                    Is.Empty);
                Assert.That(
                    _dbContext.Themes.Where(th => th.CatalogId == 1),
                    Is.Empty);
            });
        }

        /// <summary>
        /// Arange: Создаем базу с данными, контроллер
        /// Act: Запрашиваем удаление каталога с несуществующей id
        /// Assert: Ожидаем NotFoundResult
        /// </summary>
        [Test]
        public async Task DeleteCatalog_NotExistsCatalogId_ShouldReturnsNotFound()
        {
            await GenerateDbData();
            CatalogsController codeUnderTest = new(_dbContext);
            var result = await codeUnderTest.DeleteCatalog(100);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}
