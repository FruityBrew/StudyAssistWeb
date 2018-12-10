using System;
using System.Collections.Generic;
using System.Text;

namespace StudyAssist.Infrastructure.Data.DataModel
{
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

        public int ThemeId { get; set; }

        /// <summary>
        /// Название темы.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Коллекция проблем.
        /// </summary>
        public List<Problem> Problems { get; set; }

        public Category Category { get; set; }

        #endregion Properties
    }
}
