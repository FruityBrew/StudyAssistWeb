using System.ComponentModel.DataAnnotations;

namespace StudyAssist.Model
{
    /// <summary>
    /// Элемент вопроса темы
    /// </summary>
    public class Issue
    {
        public int? IssueId { get; set; }

        public int? ThemeId { get; set; }

        [MaxLength(200)]
        public required string Question { get; set; }

        public string? Answer { get; set; }

        public Theme? Theme { get; set; }
    }
}
