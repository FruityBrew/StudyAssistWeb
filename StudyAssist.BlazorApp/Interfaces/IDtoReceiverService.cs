using StudyAssist.BlazorApp.ViewModels;

namespace StudyAssist.BlazorApp.Interfaces
{
    public interface IDtoReceiverService
    {
        Task<List<CatalogVm>> GetCatalogTreeAsync();

        Task<EditingIssueVm> GetEditingIssueAsync();
    }
}
