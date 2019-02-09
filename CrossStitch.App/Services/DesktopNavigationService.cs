using CrossStitch.Core.Services;
using System.Windows.Controls;

namespace CrossStitch.App.Services
{
    public class DesktopNavigationService : NavigationServiceBase
    {
        public DesktopNavigationService(Frame frame)
        {
            Frame = frame;
        }

        public Frame Frame { get; set; }

        public override void GoBack()
        {
            if (!Frame.CanGoBack)
                return;

            Frame.GoBack();
        }

        protected override void PerformNavigation(object view)
        {
            Frame.Navigate(view);
        }
    }
}