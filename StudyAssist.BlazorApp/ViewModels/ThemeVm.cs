namespace StudyAssist.BlazorApp.ViewModels
{
    public class ThemeVm
    {
        public int Id { get; set; } 
        public string Name { get; set; }

        public List<IssueVm> Issues { get; set; }
    }
}
