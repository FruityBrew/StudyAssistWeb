using Microsoft.AspNetCore.Components;
using Utilities;

namespace StudyAssist.BlazorApp.ViewModels
{
    internal class MainViewModel
    {

        #region fields

        private List<CatalogVm> _catalogs;
        private EditingIssueVm _editingIssue;
        internal EditingIssueVm _defaultIssue;
        internal ThemeVm _defaultTheme;
        internal CatalogVm _defaultCatalog;
        internal ItemVm _defaultItem;
        private object _currentItem;
        private string _repeatCountText = @"Количество повторений: -";
        private string _repeatDateText = @"Дата повтора: -";
        private CatalogVm _selectedCatalog;
        private ThemeVm _selectedTheme;
        private ItemVm _selectedItem;
        internal bool _f = true;
        private bool _isThemeVisible = false;
        private bool _isIssueVisible = false;
        private bool _isCatalogVisible = false;
        private bool _isCatalogDisabled = true;
        private bool _isThemeDisabled = false;
        private bool _isIssueDisabled = false;
        internal string _expandedCatalog;
        internal string _expandedTheme;
        internal TypingTimer _timer;

        #endregion fields

        #region properties

        internal bool IsNotStudy
        {
            get
            {
                return !EditingIssue.IsStudy;
            }
            set
            {

            }
        }

        internal List<CatalogVm> Catalogs 
         { 
            get => _catalogs; 
            private set => _catalogs = value; 
        }

        internal EditingIssueVm EditingIssue 
        { 
            get => _editingIssue; 
            set => _editingIssue = value; 
        }

        internal object CurrentItem 
        { 
            get => _currentItem; 
            set => _currentItem = value; 
        }

        internal bool IsIssueVisible 
        { 
            get => _isIssueVisible; 
            set => _isIssueVisible = value; 
        }

        internal string RepeatCountText 
        { 
            get => _repeatCountText; 
            set => _repeatCountText = value; 
        }

        internal string RepeatDateText 
        { 
            get => _repeatDateText;
            set => _repeatDateText = value; 
        }

        internal bool IsCatalogVisible 
        { 
            get => _isCatalogVisible;
            set => _isCatalogVisible = value; 
        }

        internal bool IsCatalogDisabled 
        { 
            get => _isCatalogDisabled; 
            set => _isCatalogDisabled = value; 
        }

        internal bool IsThemeDisabled 
        { 
            get => _isThemeDisabled; 
            set => _isThemeDisabled = value; 
        }

        internal bool IsIssueDisabled 
        { 
            get => _isIssueDisabled; 
            set => _isIssueDisabled = value; 
        }
        
        internal CatalogVm SelectedCatalog 
        { 
            get => _selectedCatalog; 
            set => _selectedCatalog = value; 
        }

        internal bool IsThemeVisible 
        { 
            get => _isThemeVisible; 
            set => _isThemeVisible = value; 
        }

        internal ThemeVm SelectedTheme 
        { 
            get => _selectedTheme; 
            set => _selectedTheme = value; 
        }

        internal ItemVm SelectedItem 
        { 
            get => _selectedItem; 
            set => _selectedItem = value; 
        }

        #endregion properties


        internal async Task InitializeBaseCatalogAsync()
        {
            Catalogs = await AppDtoReseiver.GetCatalogsAsync();
        }

        internal async Task InitializeRepeatCatalogAsync(DateTime date)
        {
            Catalogs = await AppDtoReseiver.GetRepeatCatalogsAsync(date);
        }

        internal static async Task<MainViewModel> CreateBaseCatalogAsync()
        {
            MainViewModel instance = new MainViewModel();
            await instance.InitializeBaseCatalogAsync();

            return instance;
        }

        internal static async Task<MainViewModel> CreateRepeatCatalogAsync(DateTime date)
        {
            MainViewModel instance = new MainViewModel();
            await instance.InitializeRepeatCatalogAsync(date);

            return instance;
        }

        private MainViewModel()
        {
            _defaultCatalog = new CatalogVm();
            _defaultTheme = new ThemeVm();
            _defaultItem = new ItemVm();
            EditingIssue = new EditingIssueVm();
            _defaultIssue = new EditingIssueVm
            {
                AnswerText = string.Empty,
                IsStudy = false,
                RepeatDate = null
            };

            SelectedCatalog = new CatalogVm();
            SelectedTheme = new ThemeVm();
            SelectedItem = new ItemVm();

            _timer = new(3);
        }

        #region utilities



        internal bool Getf()
        {
            return SelectedItem == null;
        }

        internal async void CurrentItemChanged()
        {
            if(CurrentItem is CatalogVm catalog)
            {
                SelectedCatalog = Catalogs
                    .FirstOrDefault(cat => cat.Id == catalog.Id)!;
                // _isCatalogSelected = true;
                SelectedTheme = _defaultTheme;
                SelectedItem = _defaultItem;

                IsThemeVisible = false;
                IsIssueVisible = false;
                IsCatalogVisible = true;
                // _isCatalogDisabled = false;
                EditingIssue = _defaultIssue;

                // _repeatCountText = @"Количество повторений: -";
                // _repeatDateText = @"Дата повтора: -";

            }
            else if(CurrentItem is ThemeVm theme)
            {
                SelectedCatalog = Catalogs
                    .FirstOrDefault(cat => cat.Id == theme.ParentId)!;
                SelectedTheme = SelectedCatalog.Themes
                    .FirstOrDefault(th => th.Id == theme.Id)!;

                SelectedItem = _defaultItem;
                IsThemeVisible = true;
                IsIssueVisible = false;
                IsCatalogVisible = false;
                // _isCatalogDisabled = true;
                // _isThemeDisabled = false;
                EditingIssue = _defaultIssue;

                // _repeatCountText = @"Количество повторений: -";
                // _repeatDateText = @"Дата повтора: -";

                return;
            }
            else if(CurrentItem is ItemVm issueItem)
            {
                SelectedItem = issueItem;
                SelectedTheme = _FindThemeBy(issueItem.ParentId)!;
                SelectedCatalog = Catalogs
                    .FirstOrDefault(cat => cat.Id == SelectedTheme.ParentId)!;


                var res = await AppDtoReseiver.GetEditingIssue(issueItem.Id);


                if(res == null)
                {
                    EditingIssue = _defaultIssue;
                    RepeatCountText = @"Количество повторений: -";
                    RepeatDateText = @"Дата повтора: -";
                }
                else
                {
                    if(res.AnswerText == null)
                        res.AnswerText = string.Empty;

                    EditingIssue = res;
                    RepeatCountText = @"Количество повторений: " + EditingIssue.RepeatCount;
                    RepeatDateText = @"Дата повтора: " +
                        (EditingIssue.RepeatDate.HasValue
                            ? EditingIssue.RepeatDate.Value.ToString("dd.MM.yyyy")
                            : string.Empty);
                }

                IsIssueVisible = true;
                IsCatalogVisible = false;
                IsThemeVisible = false;
            }
        }

        #endregion utilities

        #region catalogs

        internal bool ExpandCatalog(object value)
        {
            return (value as CatalogVm)?.Name == _expandedCatalog;
        }

        internal async void DeleteCatalog()
        {
            await AppDtoReseiver.DeleteCatalogAsync(SelectedCatalog);
            Catalogs = await AppDtoReseiver.GetCatalogsAsync();
            // _selectedCatalog = null;

            CurrentItem = null;
        }

        internal async void CreateNewCatalog()
        {
            CatalogVm catalog = new CatalogVm
            {
                Name = "Введите название каталога..."
            };

            int newCatId = await AppDtoReseiver.AddCatalogAsync(catalog);
            Catalogs = await AppDtoReseiver.GetCatalogsAsync();
            CurrentItem = Catalogs.FirstOrDefault(cat => cat.Id == newCatId);
            CurrentItemChanged();
        }

        #endregion Catalogs

        #region Themes

        internal async void DeleteTheme()
        {
            await AppDtoReseiver.DeleteThemeAsync(SelectedTheme);
            Catalogs = await AppDtoReseiver.GetCatalogsAsync();
            CurrentItem = Catalogs.FirstOrDefault(cat => cat.Id == SelectedTheme.ParentId);

            CurrentItemChanged();
            _expandedCatalog = SelectedCatalog.Name;
        }

        internal bool ExpandTheme(object value)
        {
            return (value as ThemeVm)?.Name == _expandedTheme;
        }

        internal async void ThemeNameOnChange(string args)
        {
            await AppDtoReseiver.UpdateThemeNameAsync(SelectedTheme);
            // _catalogs = await AppDtoReseiver.GetCatalogsAsync();
        }

        internal async void CatalogNameOnChange(string args)
        {
            await AppDtoReseiver.UpdateCatalogNameAsync(SelectedCatalog);
        }

        private ThemeVm? _FindThemeBy(int themeId)
        {
            return Catalogs
                .SelectMany(cat => cat.Themes)
                .FirstOrDefault(th => th.Id == themeId);
        }

        internal async void CreateNewTheme()
        {
            // _selectedTheme = new ThemeVm();

            ThemeVm newTheme = new ThemeVm
            {
                ParentId = SelectedCatalog.Id,
                Name = "Введите наименование темы...",
            };

            int newThemeId = await AppDtoReseiver.AddThemeAsync(newTheme);

            Catalogs = await AppDtoReseiver.GetCatalogsAsync();

            _expandedCatalog = SelectedCatalog.Name;

            CurrentItem = _FindThemeBy(newThemeId)!;

            CurrentItemChanged();
        }

        #endregion Themes

        #region Issues

        internal async void CreateNewIssue()
        {
            EditingIssueVm newIssue = new EditingIssueVm
            {
                ParentId = SelectedTheme.Id,
                Name = "Введите наименование вопроса...",
                AnswerText = String.Empty
            };

            int newIssueId = await AppDtoReseiver.AddIssueAsync(newIssue);

            Catalogs = await AppDtoReseiver.GetCatalogsAsync();

            _expandedCatalog = Catalogs.FirstOrDefault(cat => cat.Id == SelectedTheme.ParentId).Name;
            _expandedTheme = SelectedTheme.Name;

            CurrentItem = _FindIssueBy(newIssueId);
        }

        internal async void IssueNameOnChange(string args)
        {
            await AppDtoReseiver.UpdateIssueNameAsync(SelectedItem);
        }

        internal async void DeleteIssue()
        {
            await AppDtoReseiver.DeleteIssueAsync(SelectedItem);
            Catalogs = await AppDtoReseiver.GetCatalogsAsync();

            ThemeVm selectedTheme = _FindThemeBy(SelectedItem.ParentId);

            SelectedItem = _defaultItem;
            EditingIssue = _defaultIssue;

            SelectedTheme = selectedTheme;
            CurrentItem = selectedTheme;
            CurrentItemChanged();
            _expandedCatalog = Catalogs.FirstOrDefault(f => f.Id == SelectedTheme.ParentId).Name;

            _expandedTheme = SelectedTheme.Name;
        }



        internal async void AddToStudy()
        {
            EditingIssue.IsStudy = true;
            await AppDtoReseiver.UpdateIssueStudyAsync(EditingIssue);
            CurrentItemChanged();
        }

        internal async void RemoveFromStudy()
        {
            EditingIssue.IsStudy = false;
            await AppDtoReseiver.UpdateIssueStudyAsync(EditingIssue);
            CurrentItemChanged();

        }

        internal async void AnswerChanged(ChangeEventArgs args)
        {
            if(_timer.IsActive == false)
                _timer.Activate(async () =>
                {
                    await AppDtoReseiver.UpdateIssueAnswerAsync(EditingIssue);
                });

        }

        private ItemVm? _FindIssueBy(int issueId)
        {
            return Catalogs
                .SelectMany(cat => cat.Themes)
                .SelectMany(t => t.Issues)
                .FirstOrDefault(th => th.Id == issueId);
        }

        internal async void SetOffIssue()
        {
            await AppDtoReseiver.UpdateIssueStudyDateAsync(EditingIssue);

            ThemeVm selectedTheme = SelectedTheme;
            CatalogVm selectedCatalog = SelectedCatalog;

            _catalogs = await AppDtoReseiver.GetRepeatCatalogsAsync(DateTime.Today);

            ThemeVm changedSelectedTheme = _FindThemeBy(SelectedItem.ParentId);
            EditingIssue = _defaultIssue;


            if(changedSelectedTheme == null)
            {
                SelectedTheme = _defaultTheme;

                CatalogVm changedSelectedCatalog = _catalogs.FirstOrDefault(cat => cat.Id == selectedTheme.ParentId);

                if(changedSelectedCatalog == null)
                {
                    SelectedCatalog = _defaultCatalog;
                    EditingIssue = _defaultIssue;
                    IsIssueVisible = false;

                    return;
                }

                CurrentItem = changedSelectedCatalog;
                _expandedCatalog = changedSelectedCatalog.Name;

                return;
            }

            _expandedCatalog = selectedCatalog.Name;
            _expandedTheme = selectedTheme.Name;

            CurrentItem = changedSelectedTheme;
        }

        #endregion Issues
    }
}
