using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudyAssistModel.DataModel
{
    /// <summary>
    /// Элемент темы раздела
    /// </summary>
    public class Theme
    {
        public  int ThemeId { get; set; }

        public int CatalogId { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public List<Issue> Issues { get; set; }

        public Catalog Catalog { get; set; }
    }
}
