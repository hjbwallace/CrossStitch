using CrossStitch.Core.Data;
using CrossStitch.Core.Interfaces;
using System;

namespace CrossStitch.Core.Services
{
    public class DatabaseContextService : IDatabaseContextService
    {
        public Func<DatabaseContext> GetContext() => () => new DatabaseContext();
    }
}