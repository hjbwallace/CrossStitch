using Serilog;
using System;
using System.Windows;

namespace CrossStitch.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var bootstrapper = new DesktopBootstrapper();
            bootstrapper.Initialise();

            Log.Information("Starting the Cross Stitch App");

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            Dispatcher.UnhandledException += OnDispatcherUnhandledException;

            AppDomain.CurrentDomain.FirstChanceException += (sender, eventArgs) =>
            {
                HandleException(eventArgs.Exception);
            };
        }

        private void HandleException(Exception exception)
        {
            Log.Error(exception, "An unhandled exception has occured {Error}");
            string errorMessage = string.Format("An unhandled exception occurred: {0}", exception?.Message);
            MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            HandleException(e.Exception);
            e.Handled = true;
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            HandleException(ex);
        }
    }
}