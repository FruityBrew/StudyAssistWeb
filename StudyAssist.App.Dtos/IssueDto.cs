namespace StudyAssist.App.Dtos
{
    public class IssueDto
    {
        public int? Id { get; set; }
        //public string Name { get; set; }

        public int? ThemeId { get; set; }

        public string AnswerText { get; set; }

        public DateTime? RepeatDate { get; set; }

        public int RepeatCount { get; set; }

        public bool IsStudy { get; set; }
    }
}
