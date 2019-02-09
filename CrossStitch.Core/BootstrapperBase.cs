﻿using CrossStitch.Core.Extensions;
using CrossStitch.Core.Helpers;
using CrossStitch.Core.Interfaces;
using CrossStitch.Core.Services;
using CrossStitch.Core.ViewModels;
using GalaSoft.MvvmLight.Ioc;

namespace CrossStitch.Core
{
    public abstract class BootstrapperBase
    {
        public void Initialise()
        {
            RegisterRepositories();
            RegisterViewModels();

            // UI
            RegisterNavigationService();
            RegisterDialogService();
            RegisterShell();

            // Configurable
            RegisterContextFactory();
            RegisterDateTimeProvider();
            RegisterBackupService();
        }

        protected virtual void RegisterBackupService()
        {
            SimpleIoc.Default.Register<IBackupService, BackupService>();
        }

        protected virtual void RegisterContextFactory()
        {
            SimpleIoc.Default.Register<IDatabaseContextService, DatabaseContextService>();
        }

        protected virtual void RegisterDateTimeProvider()
        {
            SimpleIoc.Default.Register<ICurrentDateService, CurrentDateService>();
        }

        protected abstract void RegisterDialogService();

        protected abstract void RegisterNavigationService();

        protected abstract void RegisterShell();

        private static void RegisterRepositories()
        {
            typeof(IRepository).Assembly.GetLoadableTypes().ConfigureImplementations<IRepository>();
        }

        private static void RegisterViewModels()
        {
            typeof(ViewModelBase).Assembly.GetLoadableTypes().ConfigureImplementations<ViewModelBase>();
        }
    }
}