using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ninject;
using StudyAssist.Core.Interfaces;
using StudyAssist.Domain.Interfaces;
using StudyAssist.Infrastructure.Data.DataModel;
using StudyAssist.Infrastructure.Util;

namespace StudyAssist.Infrastructure.Data
{
    public class CategoriesRepository : IRepository<ICategory>
    {
        #region Fields

        private readonly StudyAssistContext _dbContext;
        private DataMapper _dataMapper;


        #endregion Fields


        #region Constructors

        public CategoriesRepository()
        {
            _dbContext = new StudyAssistContext();

            _dataMapper = new DataMapper();

        }

        #endregion Constructors

        public virtual void Dispose()
        {
            _dbContext?.Dispose();
        }

        public IResult<IEnumerable<ICategory>> GetList()
        {
            var dbCategories = _dbContext.Categories
                .Include(a => a.Themes
                    .Select(b => b.Problems));

            List<ICategory> categories = dbCategories
                .AsParallel()
                .Select(_dataMapper.CreateCategoryFromData)
                .ToList();

            return new Result<IEnumerable<ICategory>>
                {
                    ReturnValue = categories,
                    Success = true
                };
        }

  

        public IResult<ICategory> Get(int id)
        {
            throw new NotImplementedException();
        }

        public IResult<int> Add(ICategory item)
        {
            Category cat = new Category
            {
                Name = item.Name
            };

            _dbContext.Categories
                .Add(cat);

            Save();

            return new Result<int>
            {
                Success = true,
                ReturnValue = cat.CategoryId
            };
        }

        public IResult Update(ICategory item)
        {
            throw new NotImplementedException();
        }

        public IResult Remove(ICategory item)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
