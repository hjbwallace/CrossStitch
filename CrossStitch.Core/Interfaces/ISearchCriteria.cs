using System;

namespace CrossStitch.Core.Interfaces
{
    public interface ISearchCriteria<T>
    {
        Func<T, bool> Apply { get; }
        bool IsValid { get; }
    }
}