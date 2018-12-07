using System;
using System.Collections.Generic;
using System.Text;
using Ninject;
using Ninject.Modules;
using StudyAssist.Core.Interfaces;
using StudyAssist.Domain.Interfaces;
using StudyAssist.Infrastructure.Util;

namespace StudyAssist.Infrastructure.Data
{
    public class ProblemRepository : IProblemRepository
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

            IProblem pr = XKernel.Instance.Get<IProblem>();
            pr.Question = "asdkjghash";

            IProblem pr2 = XKernel.Instance.Get<IProblem>();
            pr2.Question = "yyyyyyyyyyy";

            return new Result<IEnumerable<IProblem>>
            {
                Message = "All Goood!",
                Success = true,
                ReturnValue = new List<IProblem>()
                {
                    pr,
                    pr2,
                    XKernel.Instance.Get<IProblem>(),
                    XKernel.Instance.Get<IProblem>(),
                }
            };
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
