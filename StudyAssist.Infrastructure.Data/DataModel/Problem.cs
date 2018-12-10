using System;
using System.Collections.Generic;
using System.Text;

namespace StudyAssist.Infrastructure.Data.DataModel
{
    class Problem
    {
        #region Properties

        /// <summary>
        /// Идентификатор проблемы.
        /// </summary>
        public Int32? ProblemId { get; set; }

        /// <summary>
        /// Текст вопроса.
        /// </summary>
        public String Question { get; set; }

        /// <summary>
        /// Текст ответа.
        /// </summary>
        public String Answer { get; set; }

        /// <summary>
        /// Дата создания вопроса.
        /// </summary>
        public DateTime? CreationDate { get; }

        /// <summary>
        /// Дата добавления на изучение.
        /// </summary>
        public DateTime? AddedToStudyDate { get; }

        /// <summary>
        /// Расчетная дата повторения.
        /// </summary>
        public DateTime? RepeatDate { get; set; }

        /// <summary>
        /// Уровень изученности вопроса (сколько раз был повторен вопрос).
        /// </summary>
        public Byte StudyLevel { get; }

        /// <summary>
        /// Включен ли автоматический расчет даты повторения (автоповтор).
        /// </summary>
        public Boolean IsAutoRepeate { get; set; }

        /// <summary>
        /// Находится ли вопрос на изучении.
        /// </summary>
        public Boolean IsStudy { get; set; }

        public Theme Theme { get; set; }

        #endregion Properties
    }
}
