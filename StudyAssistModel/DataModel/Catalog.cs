using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudyAssistModel.DataModel
{
    /// <summary>
    /// Элемент каталаога (раздел)
    /// </summary>
    public class Catalog
    {
        public int? CatalogId { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public List<Theme> Themes { get; set; }
    }
}
