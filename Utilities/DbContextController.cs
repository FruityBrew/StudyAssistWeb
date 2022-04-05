using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Threading.Tasks;



#nullable enable

namespace Utilities
{
    /// <summary>
    /// Базовый класс для работы контроллеров апи с контекстом БД
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public class DbContextController<TContext> : ControllerBase
        where TContext : DbContext 
    {
        #region Constants

        /// <summary>
        /// Код необработанной ошибки, возникшей по вине сервера
        /// </summary>
        private const int SERVER_ERROR_CODE = 566;

        private const int CLIENT_ERROR_CODE = 466;

        #endregion Constants

        #region Fields

        protected readonly TContext _dbContext;

        #endregion Fields

        #region Constructors

        public DbContextController(TContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Метод оборачивает возвращаемые результаты в ActionResult
        /// </summary>
        /// <typeparam name="T">Тип данных</typeparam>
        /// <param name="returnValue">Данные, которые надо вернуть</param>
        /// <returns>ActionResult с данными</returns>
        protected ActionResult<T> ToResult<T>(T returnValue)
        {
            // Проверка, что значение отсутствует
            if (returnValue == null)
            {
                return NoContent();
            }

            // Проверка, что это перечисление, но в нём ничего нет
            if (returnValue is IEnumerable enumerable)
            {
                if (!enumerable.GetEnumerator().MoveNext())
                {
                    return NoContent();
                }
                else
                {
                    return new ActionResult<T>(returnValue);
                }
            }

            return new ActionResult<T>(returnValue);
        }

        /// <summary>
        /// Метод оборачивает возвращаемые результаты в Task<ActionResult>
        /// </summary>
        /// <typeparam name="T">Тип данных</typeparam>
        /// <param name="returnValue">Данные, которые надо вернуть</param>
        /// <returns>Task<ActionResult> с данными</returns>
        protected Task<ActionResult<T>> ToAsyncResult<T>(T returnValue)
        {
            return Task.FromResult(ToResult(returnValue));
        }

        protected async Task<ActionResult<R>> DoAsync<R>(
            Func<Task<ActionResult<R>>> apiAction)
        {
            try
            {
                return await apiAction();
            }
            catch (Exception e)
            {
                return ToError(e);
            }
        }

        protected async Task<ActionResult> DoAsync(
            Func<Task<ActionResult>> apiAction)
        {
            try
            {
                return await apiAction();
            }
            catch (Exception e)
            {
                return ToError(e);
            }
        }

        /// <summary>
        /// Возвращает результат с ошибкой
        /// </summary>
        /// <param name="message">Сообщение об ошибке</param>
        /// <param name="isUserMessage">
        /// Признак, что сообщение надо выводить пользователю, 
        /// иначе в консоль
        /// </param>
        /// <returns>Результат с ошибкой</returns>
        protected ObjectResult ToError(string message, bool isUserMessage = true)
        {
            return Problem(message, statusCode: 
                isUserMessage ? CLIENT_ERROR_CODE : SERVER_ERROR_CODE);
        }

        /// <summary>
        /// Вернуть результат с ошибкой
        /// </summary>
        /// <param name="exception">Информация об ошибке</param>
        /// <returns>Результат с ошибкой, которая попадёт в консоль</returns>
        protected ObjectResult ToError(Exception exception)
        {
            return Problem(exception?.ToString(), statusCode: SERVER_ERROR_CODE);
        }

        #endregion Methods
    }
}
