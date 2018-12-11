using StudyAssist.Core.Interfaces;
using StudyAssist.Domain.Interfaces;

namespace StudyAssist.Services.Interfaces
{
    public interface IStudyAssistCatalog
    {
        IResult<ICategory> GetCatalog();
    }
}
