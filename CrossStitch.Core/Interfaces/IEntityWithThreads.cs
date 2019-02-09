using CrossStitch.Core.Models;
using System.Collections.Generic;

namespace CrossStitch.Core.Interfaces
{
    public interface IEntityWithThreads { }

    public interface IEntityWithThreads<T> : IEntityWithThreads where T : ThreadBase
    {
        ICollection<T> Threads { get; set; }
    }
}