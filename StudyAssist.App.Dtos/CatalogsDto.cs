namespace StudyAssist.App.Dtos
{
    public class CatalogsDto
    {
        public CatalogsDto()
        {
            Catalogs = new();
            Themes = new();
            Issues = new();
        }

        //public Dictionary<int, string> Catalogs { get; set; }

        //public Dictionary<int, (int CatalogId, string Name)> Themes { get; set; }

        //public Dictionary<int, (int ThemeId, string Name)> Issues { get; set; }

        public List<ItemValue> Catalogs { get; set; }

        public List<ItemValue> Themes { get; set; }

        public List<ItemValue>  Issues { get; set; }
    }

    public class ItemValue
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }

        public ItemValue(int id, string name, int? parentId = null)
        {
            Id = id;
            Name = name;
            ParentId = parentId;
        }
    }
}
