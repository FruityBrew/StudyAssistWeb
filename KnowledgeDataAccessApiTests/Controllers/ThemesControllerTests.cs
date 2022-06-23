using KnowledgeDataAccessApi.Controllers;
using KnowledgeDataAccessApiTests.Utilities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using StudyAssistModel.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeDataAccessApiTests.Controllers
{
    [TestFixture]
    public class ThemesControllerTests : TestsBase
    {
        /// <summary>
        /// Arrange: Создаем контроллер с тестовой БД, содержащей данные
        /// Act: Запрашиваем список заданий по идентификатору темы.
        /// Arrange: Полученный результат должен совпадать с тем, что есть бд
        /// </summary>
        [Test]
        public async Task GetThemeIssues_ValidData_ShouldReturnsIssuesByThemesId()
        {
            await GenerateDbData();
            ThemesController codeUnderTest = new(_dbContext);

            var result = await codeUnderTest.GetThemeIssues(1);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Result, Is.Null);
                Assert.That(result.Value, Is.Not.Null.And.No.Empty);
                Assert.That(result.Value.Count, Is.EqualTo(2));
                Assert.That(result.Value[0].Question, Is.EqualTo("Вопрос"));
                Assert.That(result.Value[0].Answer, Is.EqualTo("Ответ"));
                Assert.That(result.Value[1].Question, Is.EqualTo("ВопросНН"));
                Assert.That(result.Value[1].Answer, Is.EqualTo("ОтветЩ"));
            });
        }

        /// <summary>
        /// Arrange: Добавляем в бд тему без заданий
        /// Act: Запрашиваем список заданий по идентификатору темы.
        /// Arrange: Ок резалт с пустой коллекцией
        /// </summary>
        [Test]
        public async Task GetCatalogs_NoIssuesInTheme_ShouldReturnsEmptyResult()
        {
            await GenerateDbData();
            _dbContext.Themes.Add(new Theme
            {
                CatalogId = 1
            });
            _dbContext.SaveChanges();

            ThemesController codeUnderTest = new(_dbContext);

            var result = await codeUnderTest.GetThemeIssues(2);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Result, Is.Null);
                Assert.That(result.Value, Is.Not.Null.And.Empty);
            });
        }

        /// <summary>
        /// Arrange: Создаем контроллер с тестовой БД, содержащей данные
        /// Act: Запрашиваем тему по id, не существующему в бд.
        /// Arrange: Not found result
        /// </summary>
        [Test]
        public async Task GetTheme_NoExistThemeId_ShouldReturnsNoFound()
        {
            await GenerateDbData();
            ThemesController codeUnderTest = new(_dbContext);

            var result = await codeUnderTest.GetThemeIssues(10);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Value, Is.Null);
                Assert.That(result.Result, Is.Not.Null);
                Assert.That(result.Result, Is.InstanceOf(typeof(NotFoundResult)));
            });
        }

        /// <summary>
        /// Arrange: Создаем контроллер со сгенерированной БД. Создаем добавлемую сущность
        /// Act: вызываем добавление новой сущности
        /// Arrange: должен вернуться CreatedAtActionResult с добавленной темой и ссылкой на ресурс
        /// с добавленным каталогом
        /// </summary>
        [Test]
        public async Task AddTheme_RegularWorkFlow_ShoulReturnsAddedItem()
        {
            await GenerateDbData();
            ThemesController codeUnderTest = new(_dbContext);
            Theme addedItem = new()
            {
                CatalogId = 1,
                Name = "ДжаваСкрипт"
            };

            var result = await codeUnderTest.AddTheme(addedItem);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Value, Is.Null);
                Assert.That(result.Result, Is.InstanceOf(typeof(CreatedAtActionResult)));
                var createdAtActionResult = result.Result as CreatedAtActionResult;
                Assert.That(createdAtActionResult, Is.Not.Null);
                Assert.That(
                    createdAtActionResult.Value,
                    Is.Not.Null.And.InstanceOf(typeof(Theme)));

                Theme addedItem = createdAtActionResult.Value as Theme;
                Assert.That(addedItem.Name, Is.EqualTo("ДжаваСкрипт"));
                Assert.That(addedItem.ThemeId, Is.EqualTo(2));
                Assert.That(addedItem.CatalogId, Is.EqualTo(1));
                Assert.That(
                    createdAtActionResult.ActionName,
                    Is.EqualTo("GetTheme"));
                Assert.That(
                    createdAtActionResult.RouteValues.TryGetValue(
                        "id", out object routValueResult),
                    Is.True);
                Assert.That(routValueResult.ToString(), Is.EqualTo("2"));
            });
        }

        /// <summary>
        /// Arrange: Создаем контроллер со сгенерированной БД. Создаем добавлемую сущность
        /// Act: вызываем добавление новой сущности
        /// Arrange: в бд дожна появиться запись с добавленной темой
        /// </summary>
        [Test]
        public async Task AddTheme_RegularWorkFlow_ShouldBeAddedItemInDb()
        {
            await GenerateDbData();
            ThemesController codeUnderTest = new(_dbContext);
            Theme addedItem = new()
            {
                CatalogId = 1,
                Name = "ДжаваСкрипт"
            };

            var result = await codeUnderTest.AddTheme(addedItem);
            var addedItemRes = await codeUnderTest.GetTheme(2);

            Assert.Multiple(() =>
            {
                Assert.That(addedItemRes, Is.Not.Null);
                Assert.That(addedItemRes.Result, Is.Null);
                Assert.That(addedItemRes.Value.Name, Is.EqualTo("ДжаваСкрипт"));
                Assert.That(addedItemRes.Value.CatalogId, Is.EqualTo(1));
            });
        }

        /// <summary>
        /// Arrange: Создаем базу с данными, контроллер, патч обновления имени темы
        /// Act: Вызываем обновление имени существующей темы.
        /// Assert: Должен вернуться OK
        /// </summary>
        [Test]
        public async Task UpdateThemeName_RegularWorkFlow_ShuldReturnsOk()
        {
            await GenerateDbData();
            ThemesController codeUnderTest = new(_dbContext);

            JsonPatchDocument<Theme> patch = new JsonPatchDocument<Theme>(
                new List<Operation<Theme>>
                {
                    new ("replace", "name", string.Empty, "newName"),
                },
                new DefaultContractResolver());

            var result = await codeUnderTest.UpdateThemeName(1, patch);

            Assert.That(result, Is.InstanceOf<OkResult>());
        }

        /// <summary>
        /// Arrange: Создаем базу с данными, контроллер, патч обновления темы
        /// Act: Вызываем обновление существующей темы.
        /// Assert: Сущность должна обновиться в БД
        /// </summary>
        [Test]
        public async Task UpdateTheme_RegularWorkFlow_ShouldBeUpdatedInDb()
        {
            await GenerateDbData();
            ThemesController codeUnderTest = new(_dbContext);

            JsonPatchDocument<Theme> patch = new JsonPatchDocument<Theme>(
                new List<Operation<Theme>>
                {
                    new ("replace", "name", string.Empty, "newName"),
                },
                new DefaultContractResolver());

            var result = await codeUnderTest.UpdateThemeName(1, patch);

            var updIssue = _dbContext.Themes.FirstOrDefault(f => f.ThemeId == 1);

            Assert.Multiple(() =>
            {
                Assert.That(updIssue, Is.Not.Null);
                Assert.That(updIssue.Name, Is.EqualTo("newName"));
            });
        }

        /// <summary>
        /// Arrange: Создаем базу с данными, контроллер, патч обновления имени темы
        /// Act: Вызываем обновление имени существующей темы.
        /// Assert: Должен вернуться NotFoundResult
        /// </summary>
        [Test]
        public async Task UpdateThemeName_NoExistsThemeId_ShouldReturnsNotFound()
        {
            await GenerateDbData();
            ThemesController codeUnderTest = new(_dbContext);

            JsonPatchDocument<Theme> patch = new JsonPatchDocument<Theme>(
                new List<Operation<Theme>>
                {
                    new ("replace", "name", string.Empty, "newName"),
                },
                new DefaultContractResolver());

            var result = await codeUnderTest.UpdateThemeName(100, patch);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        /// <summary>
        /// Arange: Создаем базу с данными, контроллер
        /// Act: Запрашиваем удаление темы
        /// Assert: Ожидаем НоуКонтентРезалт и отсутсвие в бд сущностей, связанных
        /// с удаляемой темой
        /// </summary>
        [Test]
        public async Task DeleteTheme_RegularWorkFlow_ShouldReturnsNoContent()
        {
            await GenerateDbData();
            ThemesController codeUnderTest = new(_dbContext);
            var result = await codeUnderTest.DeleteTheme(1);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<NoContentResult>());
                Assert.That(
                    _dbContext.Themes.Where(th => th.ThemeId == 1),
                    Is.Empty);
                Assert.That(
                    _dbContext.Issues.Where(th => th.ThemeId == 1),
                    Is.Empty);
            });
        }

        /// <summary>
        /// Arange: Создаем базу с данными, контроллер
        /// Act: Запрашиваем удаление темы с несуществующей ID
        /// Assert: Ожидаем NotFoundResult
        /// </summary>
        [Test]
        public async Task DeleteTheme_NotExistsThemeId_ShouldReturnsNotFound()
        {
            await GenerateDbData();
            ThemesController codeUnderTest = new(_dbContext);
            var result = await codeUnderTest.DeleteTheme(100);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}
