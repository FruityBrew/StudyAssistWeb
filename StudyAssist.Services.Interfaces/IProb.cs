using StudyAssist.Domain.Interfaces;
using System;
using StudyAssist.Core.Interfaces;

namespace StudyAssist.Services.Interfaces
{
    public interface IProb
    {
        IResult<IProblem> GetProblems();
    }
}
