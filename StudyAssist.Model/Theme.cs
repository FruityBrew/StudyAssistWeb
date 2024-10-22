using System.ComponentModel.DataAnnotations;

namespace StudyAssist.Model
{
	/// <summary>
	/// Элемент темы раздела
	/// </summary>
	public class Theme
    {
        public int? ThemeId { get; set; }

        public int? CatalogId { get; set; }

        [MaxLength(100)]
        public required string Name { get; set; }

        public List<Issue>? Issues { get; set; }

        public Catalog? Catalog { get; set; }
    }
}
