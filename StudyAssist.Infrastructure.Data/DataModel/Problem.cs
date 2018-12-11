using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyAssist.Infrastructure.Data.DataModel
{
    [Table("Problems")]
    class Problem
    {
        #region Properties

        /// <summary>
        /// Идентификатор проблемы.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 ProblemId { get; set; }

        /// <summary>
        /// Текст вопроса.
        /// </summary>
        [Required]
        [MaxLength(250)]
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
        [Required]
        public Byte StudyLevel { get; }

        /// <summary>
        /// Включен ли автоматический расчет даты повторения (автоповтор).
        /// </summary>
        [Required]
        public Boolean IsAutoRepeate { get; set; }

        /// <summary>
        /// Находится ли вопрос на изучении.
        /// </summary>
        [Required]
        public Boolean IsStudy { get; set; }

        [Required]
        public Theme Theme { get; set; }

        #endregion Properties
    }
}
