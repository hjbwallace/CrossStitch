using CrossStitch.Core.Services;

namespace CrossStitch.Tests.Mocks
{
    public class TestNavigationService : NavigationServiceBase
    {
        public override void GoBack()
        {
        }

        protected override void PerformNavigation(object view)
        {
        }
    }
}