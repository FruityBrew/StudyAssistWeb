using System;
using System.Collections.Generic;
using System.Text;

namespace StudyAssist.Infrastructure.Data
{
    public abstract class ServiceBase
    {
        #region Constants

        /// <summary>
        /// Шаблон сообщения об ошибке: "Ошибка на сервисе:\n\"{0}\"".
        /// </summary>
        private const string SERVICE_EXCEPTION_DETAILED_MESSAGE =
            "Ошибка на сервисе:\n\"{0}\"";

        /// <summary>
        /// Шаблон сообщения об ошибке:
        /// "Ошибка на сервисе:\n\"{0}\"\n\nВнутреннее исключение:\n\"{1}\"".
        /// </summary>
        private const string SERVICE_EXCEPTION_EXTENDED_MESSAGE =
            "Ошибка на сервисе:\n\"{0}\"\n\nВнутреннее исключение:\n\"{1}\"";

        #endregion Constants

        #region Utilities

        /// <summary>
        /// Получет сообщение об исключении.
        /// </summary>
        /// <param name="ex">Исключение.</param>
        /// <returns>Строка с текстом сообщения.</returns>
        protected String _GetExceptionMessage(Exception ex)
        {
            string message = String.Empty;

            if (ex.InnerException != null)
            {
                message = String.Format(
                    SERVICE_EXCEPTION_EXTENDED_MESSAGE,
                    ex.Message,
                    ex.InnerException.Message);
            }
            else
            {
                message = String.Format(
                    SERVICE_EXCEPTION_DETAILED_MESSAGE,
                    ex.Message);
            }

            return message;
        }

        #endregion Utilities
    }
}
