using StudyAssist.App.Api.Dtos;
using StudyAssist.BlazorApp.ViewModels;

namespace StudyAssist.BlazorApp
{
    public static class AppDtoReseiver
    {
        internal static async Task<List<CatalogVm>> GetCatalogsAsync()
        {
            CatalogsDto catalogsDto = new CatalogsDto()
            {
                Catalogs = new Dictionary<int, string>
                {
                    {1, "First" },
                    {2, "Second" },
                    {3,  "Third"}
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


            List<CatalogVm> catalogs = catalogsDto.Catalogs
                .Select(catalog => new CatalogVm 
                { 
                    Id = catalog.Key,
                    Name = catalog.Value,
                    Themes = catalogsDto.Themes
                        .Where(theme => theme.Value.CatalogId == catalog.Key)
                        .Select(theme => new ThemeVm
                        {
                            Id = theme.Key,
                            Name = theme.Value.Name,
                            Issues = catalogsDto.Issues
                                .Where(issue=> issue.Value.ThemeId == theme.Key)
                                .Select(issue => new IssueVm()
                                {
                                    Id= issue.Key,
                                    Name = issue.Value.Name,
                                }).ToList()
                        }).ToList(),
                })
                .ToList();


            return await Task.FromResult(catalogs);
        }

        internal static async Task<string> GetIssue(int issueId)
        {
            return await Task.FromResult(@"<p>Текст ответа</p>");
        }
    }
}
