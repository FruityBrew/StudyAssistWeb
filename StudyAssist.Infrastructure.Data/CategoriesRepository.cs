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

        #endregion Fields


        #region Constructors

        public CategoriesRepository()
        {
            _dbContext = new StudyAssistContext();

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
                .Select(_CreateCategoryFromData)
                .ToList();

            return new Result<IEnumerable<ICategory>>
                {
                    ReturnValue = categories,
                    Success = true
                };
        }

        private IProblem _CreateProblemFromData(Problem data)
        {
            IProblem problem = XKernel.Instance.Get<IProblem>();

            problem.Question = data.Question;
            problem.AddedToStudyDate = data.AddedToStudyDate;
            problem.Answer = data.Answer;
            problem.CreationDate = data.CreationDate;
            problem.IsAutoRepeate = data.IsAutoRepeate;
            problem.IsStudy = data.IsStudy;
            problem.ProblemId = data.ProblemId;
            problem.RepeatDate = data.RepeatDate;
            problem.StudyLevel = data.StudyLevel;

            return problem;
        }

        private ITheme _CreateThemeFromData(Theme data)
        {
            ITheme theme = XKernel.Instance.Get<ITheme>();

            theme.IsStudy = data.IsStudy;
            theme.Name = data.Name;
            theme.ThemeId = data.ThemeId;

            Parallel.ForEach(
                data.Problems,
                a => theme.AddProblem(_CreateProblemFromData(a)));

            return theme;
        }

        private ICategory _CreateCategoryFromData(Category data)
        {
            ICategory category = XKernel.Instance.Get<ICategory>();

            category.CategoryId = data.CategoryId;
            category.Name = data.Name;

            Parallel.ForEach(
                data.Themes,
                a => category.AddTheme(_CreateThemeFromData(a)));

            return category;
        }

        public IResult<ICategory> Get(int id)
        {
            throw new NotImplementedException();
        }

        public IResult<int> Add(ICategory item)
        {
            Category cat = new Category
            {
                Name = "Яволь"
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
