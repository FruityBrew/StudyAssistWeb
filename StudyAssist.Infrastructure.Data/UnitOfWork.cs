using System;
using System.Collections.Generic;
using System.Text;
using StudyAssist.Domain.Interfaces;
using StudyAssist.Infrastructure.Util;

namespace StudyAssist.Infrastructure.Data
{
    public class UnitOfWork : IDisposable
    {
        private readonly StudyAssistContext _dbContext;

        public IProblemRepository Problems { get; }

        public UnitOfWork(IProblemRepository problem)
        {
            Problems = problem;
        }

        public void Dispose()
        {
            if(_dbContext != null)
                _dbContext.Dispose();
        }
    }
}
