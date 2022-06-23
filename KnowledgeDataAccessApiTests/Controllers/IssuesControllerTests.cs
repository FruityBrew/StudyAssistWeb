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
    public class IssuesControllerTests : TestsBase
    {
        /// <summary>
        /// Arrange: Создаем контроллер с тестовой БД, содержащей данные
        /// Act: Запрашиваем задание по идентификатору.
        /// Arrange: Полученный результат должен совпадать с тем, что есть бд
        /// </summary>
        [Test]
        public async Task GetIssue_ValidData_ShouldReturnsIssueById()
        {
            await GenerateDbData();
            IssuesController codeUnderTest = new(_dbContext);

            var result = await codeUnderTest.GetIssue(2);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Result, Is.Null);
                Assert.That(result.Value.Question, Is.EqualTo("ВопросНН"));
                Assert.That(result.Value.Answer, Is.EqualTo("ОтветЩ"));
            });
        }

        /// <summary>
        /// Arrange: Создаем контроллер с тестовой БД, содержащей данные
        /// Act: Запрашиваем задачу по идентификатору, не существующего в бд
        /// Arrange: Not found result
        /// </summary>
        [Test]
        public async Task GetIssue_NotExistIssueId_ShouldReturnNoFound()
        {
            await GenerateDbData();
            IssuesController codeUnderTest = new(_dbContext);

            var result = await codeUnderTest.GetIssue(200);

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
        /// Arrange: должен вернуться CreatedAtActionResult с добавленной задачей и 
        /// ссылкой на ресурс с добавленной задачей
        /// </summary>
        [Test]
        public async Task AddIssue_RegularWorkFlow_ShouldReturnAddedItem()
        {
            await GenerateDbData();
            IssuesController codeUnderTest = new(_dbContext);

            var result = await codeUnderTest.AddIssue(new Issue
            {
                Question = "Который час?",
                Answer = "qwerty",
                ThemeId = 1
            });

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Value, Is.Null);
                Assert.That(result.Result, Is.InstanceOf(typeof(CreatedAtActionResult)));

                var createdAtActionResult = result.Result as CreatedAtActionResult;
                Assert.That(
                    createdAtActionResult.Value,
                    Is.Not.Null.And.InstanceOf(typeof(Issue)));
                Issue addedItem = createdAtActionResult.Value as Issue;

                Assert.That(addedItem.Question, Is.EqualTo("Который час?"));
                Assert.That(addedItem.Answer, Is.EqualTo("qwerty"));
                Assert.That(addedItem.IssueId, Is.EqualTo(3));
                Assert.That(addedItem.ThemeId, Is.EqualTo(1));

                Assert.That(
                    createdAtActionResult.ActionName,
                    Is.EqualTo("GetIssue"));
                Assert.That(
                    createdAtActionResult.RouteValues.TryGetValue(
                        "id", out object routValueResult),
                    Is.True);
                Assert.That(routValueResult.ToString(), Is.EqualTo("3"));
            });
        }

        /// <summary>
        /// Arrange: Создаем контроллер со сгенерированной БД. Создаем добавлемую сущность
        /// Act: вызываем добавление новой сущности
        /// Arrange: в бд дожна появиться запись с добавленной задачей
        /// </summary>
        [Test]
        public async Task AddTheme_RegularWorkFlow_ShouldBeAddedItemInDb()
        {
            await GenerateDbData();
            IssuesController codeUnderTest = new(_dbContext);

            var result = await codeUnderTest.AddIssue(new Issue
            {
                Question = "Который час?",
                Answer = "qwerty",
                ThemeId = 1
            });

            var addedItem = _dbContext.Issues.FirstOrDefault(f => f.IssueId == 3);

            Assert.Multiple(() =>
            {
                Assert.That(addedItem, Is.Not.Null);
                Assert.That(addedItem.Question, Is.EqualTo("Который час?"));
                Assert.That(addedItem.Answer, Is.EqualTo("qwerty"));
                Assert.That(addedItem.IssueId, Is.EqualTo(3));
                Assert.That(addedItem.ThemeId, Is.EqualTo(1));
            });
        }

        /// <summary>
        /// Arrange: Создаем базу с данными, контроллер, патч обновления задачи
        /// Act: Вызываем обновление существующей задачи.
        /// Assert: Должен вернуться OK
        /// </summary>
        [Test]
        public async Task UpdateIssue_RegularWorkFlow_ShouldReturnsOk()
        {
            await GenerateDbData();
            IssuesController codeUnderTest = new(_dbContext);

            JsonPatchDocument<Issue> patch = new JsonPatchDocument<Issue>(
                new List<Operation<Issue>>
                {
                    new ("replace", "question", string.Empty, "wtf??"),
                    new ("replace", "answer", string.Empty, "fuckOff!!"),
                },
                new DefaultContractResolver());
            var result = await codeUnderTest.UpdateIssue(1, patch);

            Assert.That(result, Is.InstanceOf(typeof(OkResult)));
        }

        /// <summary>
        /// Arrange: Создаем базу с данными, контроллер, патч обновления задачи
        /// Act: Вызываем обновление существующей задачи.
        /// Assert: Сущность должна обновиться в БД
        /// </summary>
        [Test]
        public async Task UpdateIssue_RegularWorkFlow_ShouldBeUpdatedInDb()
        {
            await GenerateDbData();
            IssuesController codeUnderTest = new(_dbContext);

            JsonPatchDocument<Issue> patch = new JsonPatchDocument<Issue>(
                new List<Operation<Issue>>
                {
                    new ("replace", "question", string.Empty, "wtf??"),
                    new ("replace", "answer", string.Empty, "fuckOff!!"),
                },
                new DefaultContractResolver());
            var result = await codeUnderTest.UpdateIssue(1, patch);

            var updIssue = _dbContext.Issues.FirstOrDefault(f => f.IssueId == 1);

            Assert.Multiple(() =>
            {
                Assert.That(updIssue, Is.Not.Null);
                Assert.That(updIssue.Answer, Is.EqualTo("fuckOff!!"));
                Assert.That(updIssue.Question, Is.EqualTo("wtf??"));
            });
        }

        /// <summary>
        /// Arrange: Создаем базу с данными, контроллер, патч обновления задачи
        /// Act: Вызываем обновление задачи с несуществующим Id
        /// Assert: Должен вернуться NotFoundResult
        /// </summary>
        [Test]
        public async Task UpdateIssueNotExistIssueId_ShouldReturnsNotFound()
        {
            await GenerateDbData();
            IssuesController codeUnderTest = new(_dbContext);

            JsonPatchDocument<Issue> patch = new JsonPatchDocument<Issue>(
                new List<Operation<Issue>>
                {
                    new ("replace", "question", string.Empty, "wtf??"),
                    new ("replace", "answer", string.Empty, "fuckOff!!"),
                },
                new DefaultContractResolver());
            var result = await codeUnderTest.UpdateIssue(10, patch);

            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));
        }

        /// <summary>
        /// Arange: Создаем базу с данными, контроллер
        /// Act: Запрашиваем удаление задачи
        /// Assert: Ожидаем НоуКонтентРезалт и отсутсвие в бд сущностей, связанных
        /// с удаляемой задачей
        /// </summary>
        [Test]
        public async Task DeleteIssue_RegularWorkFlow_ShouldReturnsNoContent()
        {
            await GenerateDbData();
            IssuesController codeUnderTest = new(_dbContext);
            var result = await codeUnderTest.DeleteIssue(1);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<NoContentResult>());
                Assert.That(
                    _dbContext.Issues.Where(iss => iss.IssueId == 1),
                    Is.Empty);
                Assert.That(
                    _dbContext.IssuesUnderStudy.Where(iss => iss.IssueId == 1),
                    Is.Empty);
            });
        }

        /// <summary>
        /// Arange: Создаем базу с данными, контроллер
        /// Act: Запрашиваем удаление задачи с несуществующим ID
        /// Assert: Ожидаем NotFoundResult
        /// </summary>
        [Test]
        public async Task DeleteIssue_NotExistsIssueId_ShouldReturnsNotFound()
        {
            await GenerateDbData();
            IssuesController codeUnderTest = new(_dbContext);
            var result = await codeUnderTest.DeleteIssue(100);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}
