using System;
using System.Linq.Expressions;

namespace CrossStitch.Core.Interfaces
{
    public interface IRepository
    {
        void Clear();
    }

    public interface IRepository<T> : IRepository
    {
        T Get(object key);

        T[] GetAll();

        T[] Query(Expression<Func<T, bool>> expression);

        void Remove(params T[] entities);

        void Save(params T[] entities);
    }
}