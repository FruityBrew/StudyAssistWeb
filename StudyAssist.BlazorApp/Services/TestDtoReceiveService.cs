using StudyAssist.App.Dtos;
using StudyAssist.BlazorApp.Interfaces;
using StudyAssist.BlazorApp.ViewModels;
using StudyAssist.Model;

namespace StudyAssist.BlazorApp.Services
{
    public class TestDtoReceiveService : IDtoReceiverService
    {
        private CatalogsDto _catalogsDto = new CatalogsDto()
        {
            Catalogs = new List<ItemValue>
                {
                    new (1, "First"),
                    new (2, "Second" ),
                    new(3,  "Third"),
                },
            Themes =
                {
                    new(1, "Theme11", 1 ),
                    new(2, "Theme12", 1 ),
                    new(3, "Theme13", 1 ),
                    new(4, "Theme24", 2 ),
                    new(5, "Theme25", 2 ),
                    new(6, "Theme36", 3 ),
                },
            Issues =
                {
                    new (1,"Issue111", 1),
                    new (2,"Issue112", 1),
                    new (3,"Issue123 Очень длинное название Очень длинное название Очень длинное название Очень длинное название ", 2),
                    new (4,"Issue134", 3),
                    new (5,"Issue135", 3),
                    new (6,"Issue246", 4),
                    new (7,"Issue247", 4),
                    new (8,"Issue118", 1),
                    new (9,"Issue119", 1),
                }
        };

        public  async Task<List<CatalogVm>> GetCatalogTreeAsync()
        {
            List<CatalogVm> catalogs = _catalogsDto.Catalogs
                .Select(catalog => new CatalogVm
                {
                    Id = catalog.Id,
                    Name = catalog.Name,
                    Themes = _catalogsDto.Themes
                        .Where(theme => theme.ParentId == catalog.Id)
                        .Select(theme => new ThemeVm
                        {
                            Id = theme.Id,
                            Name = theme.Name,
                            ParentId = theme.ParentId ?? 0,
                            Issues = _catalogsDto.Issues
                                .Where(issue => issue.ParentId == theme.Id)
                                .Select(issue => new ItemVm()
                                {
                                    Id = issue.Id,
                                    Name = issue.Name,
                                    ParentId = theme.ParentId ?? 0
                                }).ToList()
                        }).ToList(),
                })
                .ToList();


            return await Task.FromResult(catalogs);
        }

        public async Task<EditingIssueVm> GetEditingIssueAsync(int issueId)
        {
            return await Task.FromResult(_editingIssueVms.FirstOrDefault(f => f.Id == issueId));
        }

        public async Task<int> AddThemeAsync(ThemeVm theme)
        {
            int themeId = _catalogsDto.Themes.Max(th => th.Id) + 1;
            _catalogsDto.Themes.Add(new ItemValue(themeId, theme.Name, theme.ParentId));
            return themeId;
        }

        public async Task UpdateThemeNameAsync(ThemeVm source)
        {
            ItemValue? taregetRemoving = _catalogsDto.Themes.FirstOrDefault(th => th.Id == source.Id);
            _catalogsDto.Themes.Remove(taregetRemoving);

            _catalogsDto.Themes.Add(new ItemValue( source.Id, source.Name, source.ParentId));
        }

        public async Task<int> AddCatalogAsync(CatalogVm catalog)
        {
            int catalogId = _catalogsDto.Catalogs.Max(c => c.Id) + 1;
            _catalogsDto.Catalogs.Add(new (catalogId, catalog.Name));

            return catalogId;
        }

        public async Task UpdateCatalogNameAsync(CatalogVm source)
        {
            ItemValue? taregetRemoving = _catalogsDto.Themes.FirstOrDefault(i => i.Id == source.Id);

            _catalogsDto.Catalogs.Remove(taregetRemoving);
            _catalogsDto.Catalogs.Add(new(source.Id, source.Name));
        }

        public async Task DeleteCatalogAsync(CatalogVm source)
        {
            var themes = _catalogsDto.Themes
                .Where(th => th.ParentId == source.Id);
            //.Select(th => th.Id);

            //var issues = _catalogsDto.Issues
            //    .Where(i => themes.Any(t => t.Id == i.ParentId));
            //.Select(i => i.Key);

            _catalogsDto.Issues.RemoveAll(i => themes.Any(t => t.Id == i.ParentId));

            //foreach (var issue in issues)
            //{
            //    _catalogsDto.Issues.Remove(issue);
            //}

            foreach (var theme in themes)
            {
                _catalogsDto.Themes.Remove(theme);
            }

            _catalogsDto.Catalogs.RemoveAll(cat => cat.Id == source.Id);

        }

        public async Task DeleteThemeAsync(ThemeVm source)
        {
            //var issues = _catalogsDto.Issues
            //    .Where(i => source.ParentId == i.Value.ThemeId)
            //    .Select(i => i.Key);

            //foreach (var issue in issues)
            //{
            //    _catalogsDto.Issues.Remove(issue);
            //}

            _catalogsDto.Issues.RemoveAll(i =>  i.ParentId == source.Id);

            _catalogsDto.Themes.RemoveAll(t => t.Id == source.Id);
        }

        public  async Task DeleteIssueAsync(ItemVm deleted)
        {
            //var delItem = _catalogsDto.Issues
            //    .FirstOrDefault(i => i.Key == deleted.Id);

            _catalogsDto.Issues.RemoveAll(i => i.Id == deleted.Id);

            //var delIssue = _editingIssueVms.FirstOrDefault(f => f.Id == deleted.Id);

            //if (delIssue != null)
            //    _editingIssueVms.Remove(delIssue);

            _editingIssueVms.RemoveAll(ivm => ivm.Id == deleted.Id);
        }


        public async Task UpdateIssueNameAsync(ItemVm source)
        {
            //var target = _catalogsDto.Issues
            //    .FirstOrDefault(t => t.Key == source.Id);

            //_catalogsDto.Issues.Remove(source.Id);

            _catalogsDto.Issues.RemoveAll(i => i.Id == source.Id);
            _catalogsDto.Issues.Add(new ItemValue( source.Id, source.Name, source.ParentId));
        }

        public async Task UpdateIssueAnswerAsync(EditingIssueVm issueVm)
        {
            EditingIssueVm editingIssue = _editingIssueVms.FirstOrDefault(f => f.Id == issueVm.Id);
            if (editingIssue == null)
                return;

            editingIssue.AnswerText = issueVm.AnswerText;
        }

        public async Task UpdateIssueStudyAsync(EditingIssueVm issueVm)
        {
            EditingIssueVm editingIssue = _editingIssueVms.FirstOrDefault(f => f.Id == issueVm.Id);
            if (editingIssue == null)
                return;

            editingIssue.IsStudy = issueVm.IsStudy;

            if (editingIssue.IsStudy == false)
            {
                editingIssue.RepeatDate = null;
                editingIssue.RepeatCount = 0;
            }
            else
            {
                editingIssue.RepeatDate = DateTime.Now.AddDays(7);
            }
        }

        public async Task<int> AddIssueAsync(EditingIssueVm issue)
        {
            int issueId = _catalogsDto.Issues.Max(i => i.Id) + 1;
            _catalogsDto.Issues.Add(new ItemValue(issueId, issue.Name, issue.ParentId));
            issue.Id = issueId;
            _editingIssueVms.Add(issue);

            return issueId;
        }

        public async Task<List<CatalogVm>> GetRepeatCatalogTreeAsync(DateTime date)
        {
            List<CatalogVm> catalogs = _catalogsDto.Catalogs
                            .Select(catalog => new CatalogVm
                            {
                                Id = catalog.Id,
                                Name = catalog.Name,
                                Themes = _catalogsDto.Themes
                                    .Where(theme => theme.ParentId == catalog.Id)
                                    .Select(theme => new ThemeVm
                                    {
                                        Id = theme.Id,
                                        Name = theme.Name,
                                        ParentId = catalog.Id,
                                        Issues = _catalogsDto.Issues
                                            .Where(issue => issue.ParentId == theme.Id)
                                            .Where(issue => _editingIssueVms
                                                                                                    .Where(issueVm => issueVm.IsStudy
                                                                                                        && issueVm.RepeatDate <= date)
                                                                                                    .Select(issueVm => issueVm.Id)
                                                                                                    .Contains(issue.Id))
                                            .Select(issue => new ItemVm()
                                            {
                                                Id = issue.Id,
                                                Name = issue.Name,
                                                ParentId = theme.ParentId ?? 0
                                            }).ToList()
                                    })
                                    .Where(theme => theme.Issues.Count > 0)
                                    .ToList(),
                            })
                            .Where(catalog => catalog.Themes.Any())
                            .ToList();


            return await Task.FromResult(catalogs);
        }

        public async Task ProlongIssueStudyDateAsync(EditingIssueVm issueVm)
        {
            EditingIssueVm editingIssue = _editingIssueVms.FirstOrDefault(f => f.Id == issueVm.Id);
            if (editingIssue == null)
                return;

            editingIssue.RepeatDate = DateTime.Now.AddDays(7);
            editingIssue.RepeatCount++;
        }

        private  List<EditingIssueVm> _editingIssueVms = new List<EditingIssueVm>
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
            },
            new EditingIssueVm
            {
                Id = 1,
                Name = "Issue111",
                RepeatCount = 1,
                RepeatDate = DateTime.Today.AddDays(-1),
                IsStudy = true,
                AnswerText = @"<p>Yfdrf<p/>"
            },
            new EditingIssueVm
            {
                Id = 4,
                Name = "Issue134",
            },
                        new EditingIssueVm
            {
                Id = 5,
                Name = "Issue135",
            },
            new EditingIssueVm
            {
                Id = 6,
                Name = "Issue246",
            },
                        new EditingIssueVm
            {
                Id = 7,
                Name = "Issue247",
            },
                        new EditingIssueVm
            {
                Id = 8,
                Name = "Issue118",
            },
            new EditingIssueVm
            {
                Id = 9,
                Name = "Issue119",
            },
        };
    }
}
