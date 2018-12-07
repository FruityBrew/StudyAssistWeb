using StudyAssist.Core.Interfaces;
using StudyAssist.Domain.Interfaces;

namespace StudyAssist.Services.Interfaces
{
    public interface IProblemService
    {
        IResult<IProblem> GetProblems();
    }
}
