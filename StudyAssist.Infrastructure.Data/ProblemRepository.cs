using StudyAssist.Core.Interfaces;
using StudyAssist.Domain.Interfaces;
using StudyAssist.Infrastructure.Data.DataModel;
using System;
using System.Linq;

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

        private const String NULL_ID_ERR = "Отсутсвует идентификатор проблемы";

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

        public IResult Update(IProblem updItem)
        {
            Result result = new Result();

            try
            {
                Result check = _CheckProblem(updItem);

                if (check.Success == false)
                    return check;

                _UpdateProblem(updItem);

                result.Success = true;
            }
            catch (Exception e)
            {
                result.Message = _GetExceptionMessage(e);
            }

            return result;
        }

        public IResult Remove(IProblem removedItem)
        {
            Result result = new Result();

            try
            {
                _RemoveProblem(removedItem);

                result.Success = true;
            }
            catch (Exception e)
            {
                result.Message = _GetExceptionMessage(e);
            }

            return result;
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        #endregion IRepository<IProblem>

        #region  Utilities

        private void _RemoveProblem(IProblem removedItem)
        {
            if(removedItem == null)
                throw new ArgumentNullException(nameof(removedItem));

            Problem dbItem = _GetProblemFromDb(removedItem);

            _dbContext.Problems.Remove(dbItem);

            _dbContext.SaveChanges();
        }

        private IProblem _GetProblem(Int32 problemId)
        {
            Problem dbItem = _GetProblemFromDb(problemId);

            return _mapper.CreateProblemFromData(dbItem);
        }

        private Problem _GetProblemFromDb(IProblem problem)
        {
            if(problem.ProblemId.HasValue == false)
                throw new InvalidOperationException(NULL_ID_ERR);

            Int32 problemId = problem.ProblemId.Value;

            return _GetProblemFromDb(problemId);
        }

        private Problem _GetProblemFromDb(Int32 problemId)
        {
            Problem dbItem = _dbContext.Problems
                .FirstOrDefault(a => a.ProblemId == problemId);

            if (dbItem == null)
                throw new InvalidOperationException(
                    "Элемент с указанным идентификатором не найден");

            return dbItem;
        }

        private Int32 _AddProblem(IProblem addedItem)
        {
            Problem data = _mapper.CreateDataFromProblem(addedItem);

            _dbContext.Problems.Add(data);

            _dbContext.SaveChanges();

            return data.ProblemId;
        }

        private void _UpdateProblem(IProblem updItem)
        {
            if(updItem == null)
                throw new ArgumentNullException(nameof(updItem));

            Problem data = _GetProblemFromDb(updItem);

            _mapper.UpdateDataFromProblem(data, updItem);

            _dbContext.SaveChanges();
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
