using System;
using System.Collections.Generic;
using System.Text;

namespace StudyAssist.Domain.Interfaces
{
    public interface IResult
    {
        String Message { get; }

        Boolean Success { get; }
    }

    public interface IResult<out T> : IResult
    {
        T ReturnValue { get; }
    }
}
