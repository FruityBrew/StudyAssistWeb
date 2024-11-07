namespace StudyAssist.App.Api.Dtos
{
    public class CatalogsDto
    {
        public CatalogsDto() 
        {
            Catalogs = new();
            Themes = new();
            Issues = new();
        }

        public Dictionary<int, string> Catalogs { get; set; }

        public Dictionary<int, (int CatalogId, string Name)> Themes { get; set; }

        public Dictionary<int, (int ThemeId, string Name)> Issues { get; set; }
    }
}
