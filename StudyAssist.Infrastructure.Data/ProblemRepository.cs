using System;
using System.Collections.Generic;
using System.Text;
using StudyAssist.Core.Interfaces;
using StudyAssist.Domain.Interfaces;

namespace StudyAssist.Infrastructure.Data
{
    public class ProblemRepository : IRepository<IProblem>
    {
        #region Fields

        private readonly StudyAssistContext _dbContext;

        #endregion Fields

        #region Constructors

        public ProblemRepository()
        {
            _dbContext = new StudyAssistContext();
        }

        #endregion Constructors

        #region IRepository<IProblem>

        public virtual void Dispose()
        {
            _dbContext?.Dispose();
        }

        public IResult<IEnumerable<IProblem>> GetList()
        {
            throw new NotImplementedException();
        }

        public IResult<IProblem> Get(int id)
        {
            throw new NotImplementedException();
        }

        public IResult<int> Add(IProblem item)
        {
            throw new NotImplementedException();
        }

        public IResult Update(IProblem item)
        {
            throw new NotImplementedException();
        }

        public IResult Remove(IProblem item)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        #endregion IRepository<IProblem>




    }
}
