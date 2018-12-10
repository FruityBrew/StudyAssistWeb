using System;
using System.Collections.Generic;
using System.Text;
using StudyAssist.Core.Interfaces;
using StudyAssist.Domain.Interfaces;

namespace StudyAssist.Infrastructure.Data
{
    public class ThemeRepository : IRepository<ITheme>
    {
        #region Fields

        private StudyAssistContext _dbContext;

        #endregion Fields

        #region Constructors

        public ThemeRepository()
        {
            _dbContext = new StudyAssistContext();
        }

        #endregion Constructors

        #region IRepository<ITheme>

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IResult<IEnumerable<ITheme>> GetList()
        {
            throw new NotImplementedException();
        }

        public IResult<ITheme> Get(int id)
        {
            throw new NotImplementedException();
        }

        public IResult<int> Add(ITheme item)
        {
            throw new NotImplementedException();
        }

        public IResult Update(ITheme item)
        {
            throw new NotImplementedException();
        }

        public IResult Remove(ITheme item)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        #endregion IRepository<ITheme>
    }
}
