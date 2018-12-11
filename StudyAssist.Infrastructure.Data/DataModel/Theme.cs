using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyAssist.Infrastructure.Data.DataModel
{
    [Table("Themes")]
    class Theme
    {
        public Theme()
        {
            Problems = new List<Problem>();
        }

        #region properties

        /// <summary>
        /// Показывает находится ли тема на изучении.
        /// </summary>
        public bool IsStudy { get; set; }

        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int ThemeId { get; set; }

        /// <summary>
        /// Название темы.
        /// </summary>
        [MaxLength(200)]
        public string Name { get; set; }

        /// <summary>
        /// Коллекция проблем.
        /// </summary>
        public List<Problem> Problems { get; set; }

        [Required]
        public Category Category { get; set; }

        #endregion Properties
    }
}
