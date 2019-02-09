using CrossStitch.Core.Data;
using System;

namespace CrossStitch.Core.Interfaces
{
    public interface IDatabaseContextService
    {
        Func<DatabaseContext> GetContext();
    }
}