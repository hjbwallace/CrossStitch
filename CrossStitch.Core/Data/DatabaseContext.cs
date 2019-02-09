using CrossStitch.Core.Extensions;
using CrossStitch.Core.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace CrossStitch.Core.Data
{
    public class DatabaseContext : DbContext
    {
        private static bool _created = false;

        public DatabaseContext()
        {
            if (!_created)
            {
                SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());

                _created = true;
                OnDatabaseCreating();
            }

            Database.EnsureCreated();
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        { }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Pattern> Patterns { get; set; }
        public DbSet<PatternThread> PatternThreads { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ThreadReference> ThreadReferences { get; set; }
        protected virtual string _databaseFileName => "CrossStitchDatabase.db";

        public virtual string DatabasePath => $@".\{_databaseFileName}";

        public int CountEntries<T>()
        {
            return this.Count<T>();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($@"Data Source={DatabasePath}");
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected virtual void OnDatabaseCreating()
        {
            //Database.EnsureDeleted();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ThreadBase>()
                .ToTable("ThreadBases")
                .HasOne(x => x.ThreadReference)
                .WithMany(x => x.Threads)
                .HasForeignKey(x => x.ThreadReferenceId);

            modelBuilder.Entity<ThreadBase>()
                .HasKey(x => x.ThreadId);

            modelBuilder.Entity<PatternThread>()
                .HasOne(x => x.Pattern);

            modelBuilder.Entity<OrderThread>()
                .HasOne(x => x.Order);

            modelBuilder.Entity<ThreadReference>()
                .HasMany(c => c.Threads)
                .WithOne(e => e.ThreadReference)
                .IsRequired()
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Pattern>()
                .HasKey(x => x.PatternId);

            modelBuilder.Entity<Pattern>()
                .HasMany(x => x.Threads)
                .WithOne(x => x.Pattern)
                .HasForeignKey(x => x.PatternId);

            modelBuilder.Entity<Order>()
                .HasMany(x => x.Threads)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId);

            modelBuilder.Entity<PatternProject>()
                .ToTable("PatternProjects");

            modelBuilder.Entity<PatternProject>()
                .HasOne(bc => bc.Pattern)
                .WithMany(b => b.PatternProjects)
                .IsRequired()
                .OnDelete(DeleteBehavior.SetNull)
                .HasForeignKey(bc => bc.PatternId);

            modelBuilder.Entity<PatternProject>()
                .HasOne(bc => bc.Project)
                .WithMany(c => c.PatternProjects)
                .HasForeignKey(bc => bc.ProjectId);

            SeedData(modelBuilder);
        }

        protected virtual void SeedData(ModelBuilder modelBuilder)
        {
            var referenceData = JsonConvert.DeserializeObject<List<ThreadReference>>(File.ReadAllText(@"Resources\ThreadReferenceData.json"));
            modelBuilder.Entity<ThreadReference>().HasData(referenceData);
        }
    }
}