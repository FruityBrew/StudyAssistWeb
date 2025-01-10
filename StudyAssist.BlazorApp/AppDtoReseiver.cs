using StudyAssist.App.Api.Dtos;
using StudyAssist.BlazorApp.ViewModels;

namespace StudyAssist.BlazorApp
{
    public static class AppDtoReseiver
    {
        private static CatalogsDto _catalogsDto = new CatalogsDto()
        {
            Catalogs = new Dictionary<int, string>
                {
                    {1, "First" },
                    {2, "Second" },
                    {3,  "Third"},
                    {4, "Create new catalog" }
                },
            Themes =
                {
                    {1, (CatalogId:1, Name:"Theme11") },
                    {2, (CatalogId:1, Name:"Theme12") },
                    {3, (CatalogId:1, Name:"Theme13") },
                    {4, (CatalogId:2, Name:"Theme24") },
                    {5, (CatalogId:2, Name:"Theme25") },
                    {6, (CatalogId:3, Name:"Theme36") },
                },
            Issues =
                {
                    {1, (ThemeId:1, Name:"Issue111") },
                    {2, (ThemeId:1, Name:"Issue112") },
                    {3, (ThemeId:2, Name:"Issue123 Очень длинное название Очень длинное название Очень длинное название Очень длинное название ") },
                    {4, (ThemeId:3, Name:"Issue134") },
                    {5, (ThemeId:3, Name:"Issue135") },
                    {6, (ThemeId:4, Name:"Issue246") },
                    {7, (ThemeId:4, Name:"Issue247") },
                    {8, (ThemeId:1, Name:"Issue118") },
                    {9, (ThemeId:1, Name:"Issue119") },
                }
        };

        internal static async Task<List<CatalogVm>> GetCatalogsAsync()
        {
            List<CatalogVm> catalogs = _catalogsDto.Catalogs
                .Select(catalog => new CatalogVm 
                { 
                    Id = catalog.Key,
                    Name = catalog.Value,
                    Themes = _catalogsDto.Themes
                        .Where(theme => theme.Value.CatalogId == catalog.Key)
                        .Select(theme => new ThemeVm
                        {
                            Id = theme.Key,
                            Name = theme.Value.Name,
                            ParentId = catalog.Key,
                            Issues = _catalogsDto.Issues
                                .Where(issue=> issue.Value.ThemeId == theme.Key)
                                .Select(issue => new ItemVm()
                                {
                                    Id = issue.Key,
                                    Name = issue.Value.Name,
                                    ParentId = theme.Key
                                }).ToList()
                        }).ToList(),
                })
                .ToList();


            return await Task.FromResult(catalogs);
        }


        internal static async Task<EditingIssueVm> GetEditingIssue(int issueId)
        {
            return await Task.FromResult(_editingIssueVms.FirstOrDefault(f => f.Id == issueId));
        }

        internal static async Task<int> AddThemeAsync(ThemeVm theme)
        {
            int themeId = _catalogsDto.Themes.Max(th => th.Key) + 1;
            _catalogsDto.Themes.Add(themeId, (theme.ParentId, theme.Name));
            return themeId;
        }

        internal static async Task UpdateThemeNameAsync(ThemeVm source)
        {
            _catalogsDto.Themes.Remove(source.Id);
            _catalogsDto.Themes.Add(source.Id, (source.ParentId, source.Name));
        }

        internal static async Task UpdateCatalogNameAsync(CatalogVm source)
        {

            _catalogsDto.Catalogs.Remove(source.Id);
            _catalogsDto.Catalogs.Add(source.Id, source.Name);
        }

        internal static async Task DeleteCatalogAsync(CatalogVm source)
        {
            var themes = _catalogsDto.Themes
                .Where(th => th.Value.CatalogId == source.Id)
                .Select(th => th.Key);

            var issues = _catalogsDto.Issues
                .Where(i => themes.Contains(i.Value.ThemeId))
                .Select(i => i.Key);

            foreach(var issue in issues)
            {
                _catalogsDto.Issues.Remove(issue);
            }

            foreach(var theme in themes)
            {
                _catalogsDto.Themes.Remove(theme);
            }
            _catalogsDto.Catalogs.Remove(source.Id);

        }

        internal static async Task UpdateIssueNameAsync(ItemVm source)
        {
            var target = _catalogsDto.Issues
                .FirstOrDefault(t => t.Key == source.Id);

            _catalogsDto.Issues.Remove(source.Id);
            _catalogsDto.Issues.Add(source.Id, (source.ParentId, source.Name));
        }

        internal static async Task<int> AddIssueAsync(EditingIssueVm issue)
        {
            int issueId = _catalogsDto.Issues.Max(i => i.Key) + 1;
            _catalogsDto.Issues.Add(issueId, (issue.ParentId, issue.Name));

            return issueId;
        }

        private static List<EditingIssueVm> _editingIssueVms = new List<EditingIssueVm>
        {
            new EditingIssueVm
            {
                Id = 2,
                Name = "Issue112",
                RepeatCount = 1,
                RepeatDate = DateTime.Today.AddDays(-10),
                IsStudy = true,
                AnswerText = @"<p>Много текста<p/>"
            },
            new EditingIssueVm
            {
                Id = 3,
                Name = "Issue123 Очень длинное название Очень длинное название Очень длинное название Очень длинное название ",
                RepeatCount = 110,
                RepeatDate = DateTime.Today,
                IsStudy = true,
                AnswerText = @"<p>Очень много текста очень Много текста Ну очнь много текста Прям девать некуда сколько текста<p/>"
            }
        };
    }
}
