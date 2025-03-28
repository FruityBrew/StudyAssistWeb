using StudyAssist.BlazorApp.ViewModels;

namespace StudyAssist.BlazorApp.Interfaces
{
    public interface IDtoReceiverService
    {
        Task<List<CatalogVm>> GetCatalogTreeAsync();

        Task<List<CatalogVm>> GetRepeatCatalogTreeAsync(DateTime date);

        Task DeleteCatalogAsync(CatalogVm source);
        Task<EditingIssueVm> GetEditingIssueAsync(int issueId);

        Task<int> AddCatalogAsync(CatalogVm catalog);

        Task DeleteThemeAsync(ThemeVm source);

        Task UpdateThemeNameAsync(ThemeVm source);

        Task<int> AddThemeAsync(ThemeVm theme);

        Task UpdateCatalogNameAsync(CatalogVm source);

        Task<int> AddIssueAsync(EditingIssueVm issue);

        Task UpdateIssueStudyAsync(EditingIssueVm issueVm);

        Task UpdateIssueAnswerAsync(EditingIssueVm issueVm);

        Task UpdateIssueNameAsync(ItemVm source);

        Task DeleteIssueAsync(ItemVm deleted);

        Task ProlongIssueStudyDateAsync(EditingIssueVm issueVm);
    }
}
