using Microsoft.AspNetCore.Components;
using Utilities;

namespace StudyAssist.BlazorApp.ViewModels
{
    internal class MainViewModel
    {

        #region fields

        internal List<CatalogVm> _catalogs;
        internal EditingIssueVm _editingIssue;
        internal EditingIssueVm _defaultIssue;
        internal ThemeVm _defaultTheme;
        internal CatalogVm _defaultCatalog;
        internal ItemVm _defaultItem;
        internal object _currentItem;
        internal string _repeatCountText = @"Количество повторений: -";
        internal string _repeatDateText = @"Дата повтора: -";
        internal CatalogVm _selectedCatalog;
        internal ThemeVm _selectedTheme;
        internal ItemVm _selectedItem;
        internal bool _f = true;
        internal bool _isThemeVisible = false;
        internal bool _isIssueVisible = false;
        internal bool _isCatalogVisible = false;
        internal bool _isCatalogDisabled = true;
        internal bool _isThemeDisabled = false;
        internal bool _isIssueDisabled = false;
        internal string _expandedCatalog;
        internal string _expandedTheme;
        internal TypingTimer _timer;

        #endregion fields

        #region properties

        internal bool _IsNotStudy
        {
            get
            {
                return !_editingIssue.IsStudy;
            }
            set
            {

            }
        }

        #endregion properties


        internal async Task InitializeAsync()
        {
            _catalogs = await AppDtoReseiver.GetCatalogsAsync();

            _defaultCatalog = new CatalogVm();
            _defaultTheme = new ThemeVm();
            _defaultItem = new ItemVm();
            _editingIssue = new EditingIssueVm();
            _defaultIssue = new EditingIssueVm
            {
                AnswerText = string.Empty,
                IsStudy = false,
                RepeatDate = null
            };

            _selectedCatalog = new CatalogVm();
            _selectedTheme = new ThemeVm();
            _selectedItem = new ItemVm();

            _timer = new(3);
        }

        internal static async Task<MainViewModel> CreateAsync()
        {
            MainViewModel instance = new MainViewModel();
            await instance.InitializeAsync();

            return instance;
        }

        private MainViewModel()
        {

        }

        #region utilities



        internal bool Getf()
        {
            return _selectedItem == null;
        }

        internal async void CurrentItemChanged()
        {
            if(_currentItem is CatalogVm catalog)
            {
                _selectedCatalog = _catalogs
                    .FirstOrDefault(cat => cat.Id == catalog.Id)!;
                // _isCatalogSelected = true;
                _selectedTheme = _defaultTheme;
                _selectedItem = _defaultItem;

                _isThemeVisible = false;
                _isIssueVisible = false;
                _isCatalogVisible = true;
                // _isCatalogDisabled = false;
                _editingIssue = _defaultIssue;

                // _repeatCountText = @"Количество повторений: -";
                // _repeatDateText = @"Дата повтора: -";

            }
            else if(_currentItem is ThemeVm theme)
            {
                _selectedCatalog = _catalogs
                    .FirstOrDefault(cat => cat.Id == theme.ParentId)!;
                _selectedTheme = _selectedCatalog.Themes
                    .FirstOrDefault(th => th.Id == theme.Id)!;

                _selectedItem = _defaultItem;
                _isThemeVisible = true;
                _isIssueVisible = false;
                _isCatalogVisible = false;
                // _isCatalogDisabled = true;
                // _isThemeDisabled = false;
                _editingIssue = _defaultIssue;

                // _repeatCountText = @"Количество повторений: -";
                // _repeatDateText = @"Дата повтора: -";

                return;
            }
            else if(_currentItem is ItemVm issueItem)
            {
                _selectedItem = issueItem;
                _selectedTheme = _FindThemeBy(issueItem.ParentId)!;
                _selectedCatalog = _catalogs
                    .FirstOrDefault(cat => cat.Id == _selectedTheme.ParentId)!;


                var res = await AppDtoReseiver.GetEditingIssue(issueItem.Id);


                if(res == null)
                {
                    _editingIssue = _defaultIssue;
                    _repeatCountText = @"Количество повторений: -";
                    _repeatDateText = @"Дата повтора: -";
                }
                else
                {
                    if(res.AnswerText == null)
                        res.AnswerText = string.Empty;

                    _editingIssue = res;
                    _repeatCountText = @"Количество повторений: " + _editingIssue.RepeatCount;
                    _repeatDateText = @"Дата повтора: " +
                        (_editingIssue.RepeatDate.HasValue
                            ? _editingIssue.RepeatDate.Value.ToString("dd.MM.yyyy")
                            : string.Empty);
                }

                _isIssueVisible = true;
                _isCatalogVisible = false;
                _isThemeVisible = false;
            }
        }

        #endregion utilities

        #region catalogs

        internal bool _ExpandCatalog(object value)
        {
            return (value as CatalogVm)?.Name == _expandedCatalog;
        }

        internal async void _DeleteCatalog()
        {
            await AppDtoReseiver.DeleteCatalogAsync(_selectedCatalog);
            _catalogs = await AppDtoReseiver.GetCatalogsAsync();
            // _selectedCatalog = null;

            _currentItem = null;
        }

        internal async void _CreateNewCatalog()
        {
            CatalogVm catalog = new CatalogVm
            {
                Name = "Введите название каталога..."
            };

            int newCatId = await AppDtoReseiver.AddCatalogAsync(catalog);
            _catalogs = await AppDtoReseiver.GetCatalogsAsync();
            _currentItem = _catalogs.FirstOrDefault(cat => cat.Id == newCatId);
            CurrentItemChanged();
        }

        #endregion Catalogs

        #region Themes

        internal async void _DeleteTheme()
        {
            await AppDtoReseiver.DeleteThemeAsync(_selectedTheme);
            _catalogs = await AppDtoReseiver.GetCatalogsAsync();
            _currentItem = _catalogs.FirstOrDefault(cat => cat.Id == _selectedTheme.ParentId);

            CurrentItemChanged();
            _expandedCatalog = _selectedCatalog.Name;
        }

        internal bool _ExpandTheme(object value)
        {
            return (value as ThemeVm)?.Name == _expandedTheme;
        }

        internal async void _ThemeNameOnChange(string args)
        {
            await AppDtoReseiver.UpdateThemeNameAsync(_selectedTheme);
            // _catalogs = await AppDtoReseiver.GetCatalogsAsync();
        }

        internal async void _CatalogNameOnChange(string args)
        {
            await AppDtoReseiver.UpdateCatalogNameAsync(_selectedCatalog);
        }

        internal ThemeVm? _FindThemeBy(int themeId)
        {
            return _catalogs
                .SelectMany(cat => cat.Themes)
                .FirstOrDefault(th => th.Id == themeId);
        }

        internal async void _CreateNewTheme()
        {
            // _selectedTheme = new ThemeVm();

            ThemeVm newTheme = new ThemeVm
            {
                ParentId = _selectedCatalog.Id,
                Name = "Введите наименование темы...",
            };

            int newThemeId = await AppDtoReseiver.AddThemeAsync(newTheme);

            _catalogs = await AppDtoReseiver.GetCatalogsAsync();

            _expandedCatalog = _selectedCatalog.Name;

            _currentItem = _FindThemeBy(newThemeId)!;

            CurrentItemChanged();
        }

        #endregion Themes

        #region Issues

        internal async void _CreateNewIssue()
        {
            EditingIssueVm newIssue = new EditingIssueVm
            {
                ParentId = _selectedTheme.Id,
                Name = "Введите наименование вопроса...",
                AnswerText = String.Empty
            };

            int newIssueId = await AppDtoReseiver.AddIssueAsync(newIssue);

            _catalogs = await AppDtoReseiver.GetCatalogsAsync();

            _expandedCatalog = _catalogs.FirstOrDefault(cat => cat.Id == _selectedTheme.ParentId).Name;
            _expandedTheme = _selectedTheme.Name;

            _currentItem = _FindIssueBy(newIssueId);
        }

        internal async void _IssueNameOnChange(string args)
        {
            await AppDtoReseiver.UpdateIssueNameAsync(_selectedItem);
        }

        internal async void _DeleteIssue()
        {
            await AppDtoReseiver.DeleteIssueAsync(_selectedItem);
            _catalogs = await AppDtoReseiver.GetCatalogsAsync();

            ThemeVm selectedTheme = _FindThemeBy(_selectedItem.ParentId);

            _selectedItem = _defaultItem;
            _editingIssue = _defaultIssue;

            _selectedTheme = selectedTheme;
            _currentItem = selectedTheme;
            CurrentItemChanged();
            _expandedCatalog = _catalogs.FirstOrDefault(f => f.Id == _selectedTheme.ParentId).Name;

            _expandedTheme = _selectedTheme.Name;
        }

        internal async void _AddToStudy()
        {
            _editingIssue.IsStudy = true;
            await AppDtoReseiver.UpdateIssueStudyAsync(_editingIssue);
            CurrentItemChanged();
        }

        internal async void _RemoveFromStudy()
        {
            _editingIssue.IsStudy = false;
            await AppDtoReseiver.UpdateIssueStudyAsync(_editingIssue);
            CurrentItemChanged();

        }

        internal async void _AnswerChanged(ChangeEventArgs args)
        {
            if(_timer.IsActive == false)
                _timer.Activate(async () =>
                {
                    await AppDtoReseiver.UpdateIssueAnswerAsync(_editingIssue);
                });

        }

        internal ItemVm? _FindIssueBy(int issueId)
        {
            return _catalogs
                .SelectMany(cat => cat.Themes)
                .SelectMany(t => t.Issues)
                .FirstOrDefault(th => th.Id == issueId);
        }

        #endregion Issues
    }
}
