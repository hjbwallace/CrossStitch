using CrossStitch.App.Controls;
using CrossStitch.App.Services;
using CrossStitch.Core.Interfaces;
using CrossStitch.Core.ViewModels;
using GalaSoft.MvvmLight.Ioc;
using System.Windows;
using System.Windows.Controls;

namespace CrossStitch.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Application.Current.MainWindow = this;

            var shell = SimpleIoc.Default.GetInstance<IShell>() as Shell;
            shell.Template = this.FindResource("DefaultFrame") as ControlTemplate;
            shell.Name = "MainFrame";

            this.AddChild(shell);

            var navigationService = SimpleIoc.Default.GetInstance<INavigationService>() as DesktopNavigationService;
            navigationService.Frame = shell;

            shell.Navigate<HomeViewModel>();
        }
    }
}