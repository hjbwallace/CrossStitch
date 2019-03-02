using CrossStitch.Core.Data;
using CrossStitch.Core.Interfaces;
using System;

namespace CrossStitch.Tests.Mocks
{
    public class TestDatabaseContextService : IDatabaseContextService
    {
        public Func<DatabaseContext> GetContext()
        {
            return () => new TestDatabaseContext();
        }
    }
}