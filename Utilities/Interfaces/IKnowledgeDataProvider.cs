using StudyAssist.Model;
using System.Threading.Tasks;

namespace Utilities.Interfaces
{
    public interface IKnowledgeDataProvider
    {
        Task<int> SaveCatalog(Catalog catalog);

        Task<Catalog> SaveCatalogAsync(Catalog catalog);
    }
}
