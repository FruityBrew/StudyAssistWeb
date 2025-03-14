using StudyAssist.BlazorApp.Interfaces;
using StudyAssist.BlazorApp.ViewModels;

namespace StudyAssist.BlazorApp.Services
{
    public class AppDtoReceivedService : IDtoReceiverService
    {
        public Task<List<CatalogVm>> GetCatalogTreeAsync()
        {
            throw new NotImplementedException();
        }

        public Task<EditingIssueVm> GetEditingIssueAsync()
        {
            throw new NotImplementedException();
        }
    }
}
