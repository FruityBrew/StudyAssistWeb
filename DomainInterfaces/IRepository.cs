using System;
using System.Collections.Generic;

namespace StudyAssist.Domain.Interfaces
{
    public interface IRepository<T> : IDisposable
    {
        IResult<IEnumerable<T>> GetList();

        IResult<T> Get(Int32 id);

        IResult<Int32> Add(T item);

        IResult Update(T item);

        IResult Remove(T item);

        void Save();
    }
}
