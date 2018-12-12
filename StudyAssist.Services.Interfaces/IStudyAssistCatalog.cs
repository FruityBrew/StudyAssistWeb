using System.Collections.Generic;
using StudyAssist.Core.Interfaces;
using StudyAssist.Domain.Interfaces;

namespace StudyAssist.Services.Interfaces
{
    public interface IStudyAssistCatalog
    {
        IResult<IEnumerable<ICategory>> GetCatalog();

        IResult<IEnumerable<ICategory>> GetActiveCatalog();
    }
}
