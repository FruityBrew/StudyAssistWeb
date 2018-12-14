using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Ninject;
using Ninject.Modules;
using StudyAssist.Core.Interfaces;
using StudyAssist.Domain.Interfaces;
using StudyAssist.Infrastructure.Data.DataModel;
using StudyAssist.Infrastructure.Util;

namespace StudyAssist.Infrastructure.Data
{
    public class ProblemRepository : ServiceBase, IRepository<IProblem>
    {
        #region Constants

        private const String PROBLEM_ERR_TEMLATE = 
            "Ошибка заполнения данных вопроса: {0}";

        private const String QUESTION_NOT_SPECIFIED = 
            "Не заполнен текст вопроса.";

        private const String QUESTION_MAX_LENGTH_EXCEEDED = 
            "Текст вопроса должен состоять не более чем из 250 символов.";

        private const String ANSWER_NOT_SPECIFIED =
            "Не заполнен ответ.";

        #endregion Constants

        #region Fields

        private readonly StudyAssistContext _dbContext;

        private readonly DataMapper _mapper;

        #endregion Fields

        #region Constructors

        public ProblemRepository()
        {
            _dbContext = new StudyAssistContext();

            _mapper = new DataMapper();
        }

        #endregion Constructors

        #region IRepository<IProblem>

        public virtual void Dispose()
        {
            _dbContext?.Dispose();
        }

        public IResult<IProblem> Get(int id)
        {
            Result<IProblem> result = new Result<IProblem>();

            try
            {
                result.ReturnValue = _GetProblem(id);
                result.Success = true;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
            }

            return result;
        }

        public IResult<int> Add(IProblem item)
        {
            Result<Int32> result = new Result<int>();

            try
            {
                Result check = _CheckProblem(item);

                if (check.Success == false)
                {
                    result.Message = check.Message;

                    return result;
                }

                result.ReturnValue = _AddProblem(item);
                result.Success = true;
            }
            catch (Exception e)
            {
                result.Message = _GetExceptionMessage(e);
            }

            return result;
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

        #region  Utilities

        private IProblem _GetProblem(Int32 problemId)
        {
            Problem dbItem = _dbContext.Problems
                .FirstOrDefault(a => a.ProblemId == problemId);

            if(dbItem == null)
                throw new InvalidOperationException(
                    "Элемент с указанным идентификатором не найден");

            return _mapper.CreateProblemFromData(dbItem);
        }

        private Int32 _AddProblem(IProblem addedItem)
        {
            Problem data = _mapper.CreateDataFromProblem(addedItem);

            _dbContext.Problems.Add(data);

            return data.ProblemId;
        }

        private Result _CheckProblem(IProblem checkedItem)
        {
            if (String.IsNullOrWhiteSpace(checkedItem.Question))
                return _CreateErrResult(
                    QUESTION_NOT_SPECIFIED, PROBLEM_ERR_TEMLATE);

            if (checkedItem.Question.Length > 250)
                return _CreateErrResult(
                    QUESTION_MAX_LENGTH_EXCEEDED, PROBLEM_ERR_TEMLATE);

            return new Result(String.Empty, true);
        }

        private Result _CreateErrResult(String message, String template = null)
        {
            if(String.IsNullOrWhiteSpace(template))
                return new Result(message, false);

            return new Result(String.Format(template, message), false);
        }

        #endregion Utilities



    }
}
