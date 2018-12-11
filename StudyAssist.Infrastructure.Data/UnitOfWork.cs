using System;
using System.Collections.Generic;
using System.Text;
using StudyAssist.Core.Interfaces;
using StudyAssist.Domain.Interfaces;
using StudyAssist.Infrastructure.Data.DataModel;
using StudyAssist.Infrastructure.Util;

namespace StudyAssist.Infrastructure.Data
{
    public class UnitOfWork : IDisposable
    {
        private readonly StudyAssistContext _dbContext;

        public IRepository<IProblem> Problems { get; }

        public IRepository<ICategory> Categories { get; }

        public IRepository<ITheme> Themes { get; }

        public UnitOfWork(IRepository<IProblem> problem, 
            IRepository<ICategory> categories)
        {
            Problems = problem;
            Categories = categories;
        }

        public void Dispose()
        {
            if(_dbContext != null)
                _dbContext.Dispose();
        }
    }
}
