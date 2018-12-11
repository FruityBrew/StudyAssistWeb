using StudyAssist.Domain.Interfaces;
using System;

namespace StudyAssist.Infrastructure.Data
{
    public class Result : IResult
    {
        public string Message { get; set; }

        public bool Success { get; set; }

        public Result(String message, Boolean success)
        {
            Message = message;
            Success = success;
        }
    }

    public class Result<T> : IResult<T>
    {
        public string Message { get; set; }

        public bool Success { get; set; }

        public Result()
        { }

        public Result(String message, Boolean success)
        {
            Message = message;
            Success = success;
        }

        public T ReturnValue { get; set; }
    }
}
