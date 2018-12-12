using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StudyAssist.Core.Interfaces;
using StudyAssist.Domain.Interfaces;
using StudyAssist.Infrastructure.Data;
using StudyAssist.Infrastructure.Data.DataModel;
using StudyAssist.Services.Interfaces;

namespace StudyAssist.Infrastructure.Business
{
    public class StudyAssist : IStudyAssistCatalog, IDisposable
    {
        private StudyAssistContext _dbContext;

        private DataMapper _dataMapper;

        public StudyAssist()
        {
            _dbContext = new StudyAssistContext();
            _dataMapper = new DataMapper();
        }


        public IResult<IEnumerable<ICategory>> GetCatalog()
        {
            Result<IEnumerable<ICategory>> result = 
                new Result<IEnumerable<ICategory>>();

            try
            {
                var dbCategories = _dbContext.Categories
                    .Include(a => a.Themes
                        .Select(b => b.Problems));

                List<ICategory> categories = dbCategories
                    .AsParallel()
                    .Select(_dataMapper.CreateCategoryFromData)
                    .ToList();

                result.ReturnValue = categories;
                result.Success = true;
            }
            catch (Exception e)
            {
                //todo log
                result.Message = e.Message;
            }

            return result;
        }

        public IResult<IEnumerable<ICategory>> GetActiveCatalog()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }
    }
}