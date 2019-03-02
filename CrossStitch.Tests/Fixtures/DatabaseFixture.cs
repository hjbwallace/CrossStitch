using CrossStitch.Core.Data;
using CrossStitch.Core.Interfaces;
using CrossStitch.Tests.Mocks;
using GalaSoft.MvvmLight.Ioc;
using System;
using Xunit;

namespace CrossStitch.Tests.Fixtures
{
    [CollectionDefinition("Database collection test")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture> { }

    public class DatabaseFixture : IDisposable
    {
        private readonly Func<DatabaseContext> _contextFunc;

        public DatabaseFixture()
        {
            var bootstrapper = new TestBootstrapper();
            bootstrapper.Initialise();

            Shell = (TestShell)SimpleIoc.Default.GetInstance<IShell>();
            Navigation = (TestNavigationService)SimpleIoc.Default.GetInstance<INavigationService>();
            Dialog = (TestDialogService)SimpleIoc.Default.GetInstance<IDialogService>();

            _contextFunc = SimpleIoc.Default.GetInstance<IDatabaseContextService>().GetContext();
            _contextFunc.Invoke().Database.EnsureDeleted();
            _contextFunc.Invoke().Database.EnsureCreated();
        }

        public TestDialogService Dialog { get; }
        public TestNavigationService Navigation { get; }
        public TestShell Shell { get; }

        public void Dispose()
        {
        }
    }
}