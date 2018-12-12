using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using StudyAssist.Core.Interfaces;
using StudyAssist.Infrastructure.Data.DataModel;
using StudyAssist.Infrastructure.Util;

namespace StudyAssist.Infrastructure.Data
{
    public class DataMapper
    {
        public IProblem CreateProblemFromData(Problem data)
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

        public ITheme CreateThemeFromData(Theme data)
        {
            ITheme theme = XKernel.Instance.Get<ITheme>();

            theme.IsStudy = data.IsStudy;
            theme.Name = data.Name;
            theme.ThemeId = data.ThemeId;

            Parallel.ForEach(
                data.Problems,
                a => theme.AddProblem(CreateProblemFromData(a)));

            return theme;
        }

        public ICategory CreateCategoryFromData(Category data)
        {
            ICategory category = XKernel.Instance.Get<ICategory>();

            category.CategoryId = data.CategoryId;
            category.Name = data.Name;

            Parallel.ForEach(
                data.Themes,
                a => category.AddTheme(CreateThemeFromData(a)));

            return category;
        }
    }
}
