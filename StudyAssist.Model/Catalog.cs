using System.ComponentModel.DataAnnotations;

namespace StudyAssist.Model
{
	/// <summary>
	/// Элемент каталаога (раздел)
	/// </summary>
	public class Catalog
    {
        public int? CatalogId { get; set; }

        [MaxLength(50)]
        public required string Name { get; set; }

        public List<Theme>? Themes { get; set; }
    }
}
