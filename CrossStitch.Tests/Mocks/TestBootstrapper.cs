using CrossStitch.Core;
using CrossStitch.Core.Interfaces;
using GalaSoft.MvvmLight.Ioc;

namespace CrossStitch.Tests.Mocks
{
    public class TestBootstrapper : BootstrapperBase
    {
        protected override string LogFileFormat => @"CrossStitchTest-{Date}.log";

        protected override void RegisterContextFactory()
        {
            SimpleIoc.Default.Register<IDatabaseContextService, TestContextFactory>();
        }

        protected override void RegisterDateTimeProvider()
        {
            SimpleIoc.Default.Register<ICurrentDateService, TestCurrentDateService>();
        }

        protected override void RegisterDialogService()
        {
            SimpleIoc.Default.Register<IDialogService, TestDialogService>();
        }

        protected override void RegisterNavigationService()
        {
            SimpleIoc.Default.Register<INavigationService, TestNavigationService>();
        }

        protected override void RegisterShell()
        {
            SimpleIoc.Default.Register<IShell, TestShell>();
        }
    }
}