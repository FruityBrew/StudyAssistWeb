using System;
using System.Collections.Generic;
using System.Text;

namespace StudyAssist.Infrastructure.Data.DataModel
{
    class Category
    {

        #region ctors

        public Category()
        {
            Themes = new List<Theme>();
        }

        #endregion

        #region properties

        public int? CategoryId { get; set; }

        /// <summary>
        /// Название категории.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        public List<Theme> Themes { get; set; }


        #endregion
    }
}
