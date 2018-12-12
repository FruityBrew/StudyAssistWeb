using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace StudyAssist.Infrastructure.Data.DataModel
{
    [Table("Categories")]
    public class Category
    {
        #region ctors

        public Category()
        {
            Themes = new List<Theme>();
        }

        #endregion

        #region properties

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int CategoryId { get; set; }

        /// <summary>
        /// Название категории.
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        public List<Theme> Themes { get; set; }

        #endregion
    }
}
