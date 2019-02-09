using CrossStitch.Core.Data;
using CrossStitch.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CrossStitch.Tests.Mocks
{
    public class TestDatabaseContext : DatabaseContext
    {
        

        public TestDatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public TestDatabaseContext()
        {
        }

        protected override string _databaseFileName => "test.db";
        public override string DatabasePath => $@"{_databaseFileName}";

        protected override void OnDatabaseCreating()
        {
            Database.EnsureDeleted();
        }

        protected override void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ThreadReference>().HasData(CommonActions.ThreadReferences.Data);
        }
    }
}