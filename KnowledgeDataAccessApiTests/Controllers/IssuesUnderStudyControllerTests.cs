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
using System.Threading.Tasks;

namespace KnowledgeDataAccessApiTests.Controllers
{
    [TestFixture]
    public class IssuesUnderStudyControllerTests : TestsBase
    {
        /// <summary>
        /// Arrange: Создаем контроллер с тестовой БД, содержащей данные
        /// Act: Запрашиваем список задач на изучении.
        /// Arrange: Полученный результат должен совпадать с тем, что есть бд
        /// </summary>
        [Test]
        public async Task GetIssuesUnderStudy_RegularWorkFlow_ShouldReturnsAllIssuesUnderStudy()
        {
            await GenerateDbData();
            IssuesUnderStudyController codeUnderTest = new(_dbContext);

            var result = await codeUnderTest.GetIssuesUnderStudy();

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Result, Is.Null);
                Assert.That(result.Value, Is.Not.Null.And.No.Empty);
                Assert.That(result.Value.Count, Is.EqualTo(2));
                Assert.That(result.Value[0].RepeateDate, Is.EqualTo(DateTime.MinValue));
                Assert.That(result.Value[0].StudyLevel, Is.EqualTo(2));
                Assert.That(result.Value[0].Issue, Is.Not.Null);
                Assert.That(result.Value[0].Issue.Theme, Is.Not.Null);
                Assert.That(result.Value[0].Issue.Theme.Catalog, Is.Not.Null);
                Assert.That(result.Value[0].Issue.IssueId, Is.EqualTo(1));
                Assert.That(result.Value[1].RepeateDate, Is.EqualTo(DateTime.MaxValue));
                Assert.That(result.Value[1].StudyLevel, Is.EqualTo(1));
                Assert.That(result.Value[1].Issue, Is.Not.Null);
                Assert.That(result.Value[1].Issue.IssueId, Is.EqualTo(2));
            });
        }

        /// <summary>
        /// Arrange: Не заполняем бд, создаем контроллер с пустой бд
        /// Act: Вызываем запрос задач на изучении
        /// Arrange: Ок резалт с пустой коллекцией
        /// </summary>
        [Test]
        public async Task GetIssuesUnderStudy_EmptyDb_ShouldReturnsEmptyResult()
        {
            IssuesUnderStudyController codeUnderTest = new(_dbContext);

            var result = await codeUnderTest.GetIssuesUnderStudy();

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Result, Is.Null);
                Assert.That(result.Value, Is.Not.Null.And.Empty);
            });
        }

        /// <summary>
        /// Arrange: Создаем контроллер с тестовой БД, содержащей данные
        /// Act: Запрашиваем задание на изучении по идентификатору.
        /// Arrange: Полученный результат должен совпадать с тем, что есть бд
        /// </summary>
        [Test]
        public async Task GetIssueUnderStudy_RegularWorkFlow_ShouldReturnsIssueById()
        {
            await GenerateDbData();
            IssuesUnderStudyController codeUnderTest = new(_dbContext);

            var result = await codeUnderTest.GetIssueUnderStudy(1);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Result, Is.Null);
                Assert.That(result.Value, Is.Not.Null);
                Assert.That(result.Value.RepeateDate, Is.EqualTo(DateTime.MinValue));
                Assert.That(result.Value.StudyLevel, Is.EqualTo(2));
                Assert.That(result.Value.Issue, Is.Not.Null);
                Assert.That(result.Value.Issue.Theme, Is.Not.Null);
                Assert.That(result.Value.Issue.Theme.Catalog, Is.Not.Null);
                Assert.That(result.Value.Issue.IssueId, Is.EqualTo(1));
            });
        }

        /// <summary>
        /// Arrange: Создаем контроллер с тестовой БД, содержащей данные
        /// Act: Запрашиваем задачу на изучении по идентификатору, не существующего в бд
        /// Arrange: Not found result
        /// </summary>
        [Test]
        public async Task GetIssueUnderStudy_NotExistsIssueUnderStudyId_ShouldReturnsNotFoundResult()
        {
            await GenerateDbData();
            IssuesUnderStudyController codeUnderTest = new(_dbContext);

            var result = await codeUnderTest.GetIssueUnderStudy(10);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Result, Is.Not.Null);
                Assert.That(result.Result, Is.InstanceOf(typeof(NotFoundResult)));
            });
        }

        /// <summary>
        /// Arrange: Создаем контроллер со сгенерированной БД. Создаем добавлемую сущность
        /// Act: вызываем добавление новой сущности
        /// Arrange: должен вернуться CreatedAtActionResult с добавленной 
        /// задачей на изучении и ссылкой на неее и ссылкой на ресурс
        /// </summary>
        [Test]
        public async Task AddIssueUnderStudy_RegularWorkFlow_ShouldReturnAddedItem()
        {
            await GenerateDbData();
            IssuesUnderStudyController codeUnderTest = new(_dbContext);

            var result = await codeUnderTest.AddIssueUnderStudy(new IssueUnderStudy
            {
                IssueId = 1,
                RepeateDate = DateTime.MinValue,
                StudyLevel = 2
            });

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Value, Is.Null);
                Assert.That(result.Result, Is.InstanceOf(typeof(CreatedAtActionResult)));

                var createdAtActionResult = result.Result as CreatedAtActionResult;
                Assert.That(
                    createdAtActionResult.Value,
                    Is.Not.Null.And.InstanceOf(typeof(IssueUnderStudy)));
                IssueUnderStudy addedItem = createdAtActionResult.Value as IssueUnderStudy;

                Assert.That(addedItem.RepeateDate, Is.EqualTo(DateTime.MinValue));
                Assert.That(addedItem.IssueId, Is.EqualTo(1));
                Assert.That(addedItem.IssueUnderStudyId, Is.EqualTo(3));
                Assert.That(addedItem.StudyLevel, Is.EqualTo(2));

                Assert.That(
                    createdAtActionResult.ActionName,
                    Is.EqualTo("GetIssueUnderStudy"));
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
        /// Arrange: в бд дожна появиться запись с добавленной задачей на изучении
        /// </summary>
        [Test]
        public async Task AddIssueUnderStudy_RegularWorkFlow_ShouldBeAddedItemInDb()
        {
            await GenerateDbData();
            IssuesUnderStudyController codeUnderTest = new(_dbContext);

            var result = await codeUnderTest.AddIssueUnderStudy(new IssueUnderStudy
            {
                IssueId = 1,
                RepeateDate = DateTime.MinValue,
                StudyLevel = 2
            });

            var addedItem = _dbContext.IssuesUnderStudy
                .FirstOrDefault(f => f.IssueUnderStudyId == 3);

            Assert.Multiple(() =>
            {
                Assert.That(addedItem, Is.Not.Null);
                Assert.That(addedItem.RepeateDate, Is.EqualTo(DateTime.MinValue));
                Assert.That(addedItem.IssueId, Is.EqualTo(1));
                Assert.That(addedItem.StudyLevel, Is.EqualTo(2));
            });
        }

        /// <summary>
        /// Arrange: Создаем базу с данными, контроллер, 
        /// патч обновления задачи на изучении
        /// Act: Вызываем обновление существующей задачи на изучении.
        /// Assert: Должен вернуться OK
        /// </summary>
        [Test]
        public async Task UpdateIssueUnderStudy_RegularWorkFlow_ShouldReturnsOk()
        {
            await GenerateDbData();
            IssuesUnderStudyController codeUnderTest = new(_dbContext);

            JsonPatchDocument<IssueUnderStudy> patch = new (
                new List<Operation<IssueUnderStudy>>
                {
                    new (
                        "replace", 
                        "repeatedate", 
                        string.Empty, 
                        "2022/06/23"),
                    new ("replace", "studylevel", string.Empty, "20"),
                },
                new DefaultContractResolver());
            var result = await codeUnderTest.UpdateIssueUnderStudy(1, patch);

            Assert.That(result, Is.InstanceOf(typeof(OkResult)));
        }

        /// <summary>
        /// Arrange: Создаем базу с данными, контроллер, патч обновления 
        /// задачи на изучении
        /// Act: Вызываем обновление существующей задачи на изучении.
        /// Assert: Сущность должна обновиться в БД
        /// </summary>
        [Test]
        public async Task UpdateIssueUnderTest_RegularWorkFlow_ShouldBeUpdatedInDb()
        {
            await GenerateDbData();
            IssuesUnderStudyController codeUnderTest = new(_dbContext);

            JsonPatchDocument<IssueUnderStudy> patch = new(
                new List<Operation<IssueUnderStudy>>
                {
                    new (
                        "replace",
                        "repeatedate",
                        string.Empty,
                        "2022/06/23"),
                    new ("replace", "studylevel", string.Empty, "20"),
                },
                new DefaultContractResolver());
            var result = await codeUnderTest.UpdateIssueUnderStudy(1, patch);

            var updItem = _dbContext.IssuesUnderStudy
                .FirstOrDefault(f => f.IssueUnderStudyId == 1);

            Assert.Multiple(() =>
            {
                Assert.That(updItem, Is.Not.Null);
                Assert.That(updItem.RepeateDate, Is.EqualTo(new DateTime(2022, 6, 23)));
                Assert.That(updItem.StudyLevel, Is.EqualTo(20));
            });
        }

        /// <summary>
        /// Arrange: Создаем базу с данными, контроллер, патч обновления задачи
        /// Act: Вызываем обновление задачи на изучении с несуществующим Id
        /// Assert: Должен вернуться NotFoundResult
        /// </summary>
        [Test]
        public async Task UpdateIssueUnderStudyNotExistIssueUnderStudyId_ShouldReturnsNotFound()
        {
            await GenerateDbData();
            IssuesUnderStudyController codeUnderTest = new(_dbContext);

            JsonPatchDocument<IssueUnderStudy> patch = new(
                new List<Operation<IssueUnderStudy>>
                {
                    new (
                        "replace",
                        "repeatedate",
                        string.Empty,
                        "2022/06/23"),
                    new ("replace", "studylevel", string.Empty, "20"),
                },
                new DefaultContractResolver());
            var result = await codeUnderTest.UpdateIssueUnderStudy(10, patch);

            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));
        }

        /// <summary>
        /// Arange: Создаем базу с данными, контроллер
        /// Act: Запрашиваем удаление задачи на обучении
        /// Assert: Ожидаем НоуКонтентРезалт и отсутсвие в бд сущностей, связанных
        /// с удаляемой задачей на обучении
        /// </summary>
        [Test]
        public async Task DeleteIssueUnderStudy_RegularWorkFlow_ShouldReturnsNoContent()
        {
            await GenerateDbData();
            IssuesUnderStudyController codeUnderTest = new(_dbContext);
            var result = await codeUnderTest.DeleteIssueUnderStudy(1);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<NoContentResult>());
                Assert.That(
                    _dbContext.IssuesUnderStudy.Where(ius => ius.IssueUnderStudyId == 1),
                    Is.Empty);
            });
        }

        /// <summary>
        /// Arange: Создаем базу с данными, контроллер
        /// Act: Запрашиваем удаление задачи на обучении с несуществующим ID
        /// Assert: Ожидаем NotFoundResult
        /// </summary>
        [Test]
        public async Task DeleteIssueUnderStudy_NotExistsIssueUnderStudyId_ShouldReturnsNotFound()
        {
            await GenerateDbData();
            IssuesUnderStudyController codeUnderTest = new(_dbContext);
            var result = await codeUnderTest.DeleteIssueUnderStudy(10);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}
