using Microsoft.AspNetCore.Components;
using StudyAssist.BlazorApp.Interfaces;
using StudyAssist.BlazorApp.Services;
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

        IDtoReceiverService _receiver;



        internal async Task InitializeDefaultViewModelAsync()
        {
            Catalogs = new List<CatalogVm>();
        }

        internal async Task InitializeBaseCatalogAsync()
        {
            Catalogs = await _receiver.GetCatalogTreeAsync();
        }

        internal async Task InitializeRepeatCatalogAsync(DateTime date)
        {
            Catalogs = await _receiver.GetRepeatCatalogTreeAsync(date);
        }

        internal static async Task<MainViewModel> CreateBaseCatalogAsync(
            IDtoReceiverService receiver)
        {
            MainViewModel instance = new MainViewModel(receiver);
            await instance.InitializeBaseCatalogAsync();

            return instance;
        }

        internal static async Task<MainViewModel> CreateDefaultViewModel()
        {
            MainViewModel instance = new MainViewModel();
            return instance;
        }

        internal static async Task<MainViewModel> CreateRepeatCatalogAsync(
            IDtoReceiverService receiver,
            DateTime date)
        {
            MainViewModel instance = new MainViewModel(receiver);
            await instance.InitializeRepeatCatalogAsync(date);

            return instance;
        }

        private MainViewModel(IDtoReceiverService dtoReceiverService) : this()
        {
            _receiver = dtoReceiverService;
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

        internal async Task CurrentItemChanged()
        {
            if(CurrentItem is CatalogVm catalog)
            {
                SelectedCatalog = Catalogs
                    .FirstOrDefault(cat => cat.Id == catalog.Id)!;

                SelectedTheme = _defaultTheme;
                SelectedItem = _defaultItem;
                IsThemeVisible = false;
                IsIssueVisible = false;
                IsCatalogVisible = true;
                EditingIssue = _defaultIssue;
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
                EditingIssue = _defaultIssue;

                return;
            }
            else if(CurrentItem is ItemVm issueItem)
            {
                SelectedItem = issueItem;
                SelectedTheme = _FindThemeBy(issueItem.ParentId)!;
                SelectedCatalog = Catalogs
                    .FirstOrDefault(cat => cat.Id == SelectedTheme.ParentId)!;


                var res = await _receiver.GetEditingIssueAsync(issueItem.Id);


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
            await _receiver.DeleteCatalogAsync(SelectedCatalog);
            Catalogs = await _receiver.GetCatalogTreeAsync();
            // _selectedCatalog = null;

            CurrentItem = null;
        }

        internal async void CreateNewCatalog()
        {
            CatalogVm catalog = new CatalogVm
            {
                Name = "Введите название каталога..."
            };

            int newCatId = await _receiver.AddCatalogAsync(catalog);
            Catalogs = await _receiver.GetCatalogTreeAsync();
            CurrentItem = Catalogs.FirstOrDefault(cat => cat.Id == newCatId);
            CurrentItemChanged();
        }

        #endregion Catalogs

        #region Themes

        internal async void DeleteTheme()
        {
            await _receiver.DeleteThemeAsync(SelectedTheme);
            Catalogs = await _receiver.GetCatalogTreeAsync();
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
            await _receiver.UpdateThemeNameAsync(SelectedTheme);
            // _catalogs = await TestDtoReceiveService.GetCatalogsAsync();
        }

        internal async void CatalogNameOnChange(string args)
        {
            await _receiver.UpdateCatalogNameAsync(SelectedCatalog);
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

            int newThemeId = await _receiver.AddThemeAsync(newTheme);

            Catalogs = await _receiver.GetCatalogTreeAsync();

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

            int newIssueId = await _receiver.AddIssueAsync(newIssue);

            Catalogs = await _receiver.GetCatalogTreeAsync();

            _expandedCatalog = Catalogs.FirstOrDefault(cat => cat.Id == SelectedTheme.ParentId).Name;
            _expandedTheme = SelectedTheme.Name;

            CurrentItem = _FindIssueBy(newIssueId);
        }

        internal async void IssueNameOnChange(string args)
        {
            await _receiver.UpdateIssueNameAsync(SelectedItem);
        }

        internal async void DeleteIssue()
        {
            await _receiver.DeleteIssueAsync(SelectedItem);
            Catalogs = await _receiver.GetCatalogTreeAsync();

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
            await _receiver.UpdateIssueStudyAsync(EditingIssue);
            CurrentItemChanged();
        }

        internal async void RemoveFromStudy()
        {
            EditingIssue.IsStudy = false;
            await _receiver.UpdateIssueStudyAsync(EditingIssue);
            CurrentItemChanged();
        }

        internal async void RemoveFromStudyAndUpdateRepeatList()
        {
            EditingIssue.IsStudy = false;
            await _receiver.UpdateIssueStudyAsync(EditingIssue);

            await _UpdateCatalogAfterRemoveIssue();
        }

        internal async void SetOffIssue()
        {
            await _receiver.ProlongIssueStudyDateAsync(EditingIssue);

            await _UpdateCatalogAfterRemoveIssue();
        }

        internal async void AnswerChanged(ChangeEventArgs args)
        {
            if(_timer.IsActive == false)
                _timer.Activate(async () =>
                {
                    await _receiver.UpdateIssueAnswerAsync(EditingIssue);
                });

        }

        private ItemVm? _FindIssueBy(int issueId)
        {
            return Catalogs
                .SelectMany(cat => cat.Themes)
                .SelectMany(t => t.Issues)
                .FirstOrDefault(th => th.Id == issueId);
        }

        private async Task _UpdateCatalogAfterRemoveIssue()
        {
            ThemeVm selectedTheme = SelectedTheme;
            CatalogVm selectedCatalog = SelectedCatalog;

            _catalogs = await _receiver.GetRepeatCatalogTreeAsync(DateTime.Today);

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
