namespace StudyAssist.BlazorApp.ViewModels
{
    public class EditingIssueVm : ItemVm
    {
        public string AnswerText { get; set; }

        public DateTime? RepeatDate { get; set; }

        public int RepeatCount { get; set; }

        public bool IsStudy { get; set; }
    }
}
