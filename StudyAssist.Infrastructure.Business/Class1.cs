using System;
using StudyAssist.Core.Interfaces;
using StudyAssist.Domain.Interfaces;
using StudyAssist.Services.Interfaces;

namespace StudyAssist.Infrastructure.Business
{
    public class StudyAssist : IStudyAssistCatalog
    {
        public IResult<ICategory> GetCatalog()
        {
            throw new NotImplementedException(); 
        }
    }
}
