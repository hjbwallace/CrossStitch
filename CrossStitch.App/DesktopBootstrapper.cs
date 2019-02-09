using CrossStitch.App.Controls;
using CrossStitch.App.Services;
using CrossStitch.Core;
using CrossStitch.Core.Extensions;
using CrossStitch.Core.Helpers;
using CrossStitch.Core.Interfaces;
using GalaSoft.MvvmLight.Ioc;
using System.Windows.Controls;

namespace CrossStitch.App
{
    public class DesktopBootstrapper : BootstrapperBase
    {
        protected override void RegisterDialogService()
        {
            var dialogService = new DesktopDialogService();
            SimpleIoc.Default.Register<IDialogService>(() => dialogService);
        }

        protected override void RegisterNavigationService()
        {
            var pages = ReflectionExtensions.GetConcreteImplementations<Page>();

            foreach (var typeToRegister in pages)
                SimpleIocHelper.RegisterConcreteType(typeToRegister);

            var navigationService = new DesktopNavigationService(null);

            foreach (var page in pages)
            {
                navigationService.Configure(page.Name, page);
            }

            SimpleIoc.Default.Register<INavigationService>(() => navigationService);
        }

        protected override void RegisterShell()
        {
            SimpleIoc.Default.Register<IShell, Shell>();
        }
    }
}