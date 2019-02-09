using CrossStitch.Core.Data;
using CrossStitch.Core.Interfaces;
using CrossStitch.Tests.Mocks;
using FluentAssertions;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using Xunit;

namespace CrossStitch.Tests.Fixtures
{
    [Collection("Database collection test")]
    public abstract class DatabaseTestBase
    {
        private Func<DatabaseContext> _contextFunc;

        public DatabaseTestBase()
        {
            Shell = (TestShell)Instance<IShell>();
            Navigation = (TestNavigationService)Instance<INavigationService>();
            Dialog = (TestDialogService)Instance<IDialogService>();

            Dialog.ShownDialogs = new List<TestDialogService.DialogInformation>();

            _contextFunc = Instance<IDatabaseContextService>().GetContext();
        }

        public DatabaseContext Database => (_contextFunc.Invoke());
        public TestDialogService Dialog { get; }
        public TestNavigationService Navigation { get; }
        public TestShell Shell { get; }

        public void HasEntries<TEntry>(int expected)
        {
            Database.CountEntries<TEntry>().Should().Be(expected, typeof(TEntry).Name);
        }

        protected T Instance<T>() => SimpleIoc.Default.GetInstance<T>();
    }
}