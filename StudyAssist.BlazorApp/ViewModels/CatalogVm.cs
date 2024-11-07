namespace StudyAssist.BlazorApp.ViewModels
{
    public class CatalogVm
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<ThemeVm> Themes { get; set; }
    }
}
