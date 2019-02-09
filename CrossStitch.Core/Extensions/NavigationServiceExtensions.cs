using CrossStitch.Core.Interfaces;
using CrossStitch.Core.ViewModels;

namespace CrossStitch.Core.Extensions
{
    public static class NavigationServiceExtensions
    {
        public static ViewModelBase Navigate<T>(this INavigationService navigationService) where T : ViewModelBase
        {
            return navigationService.Navigate<T>(null);
        }

        public static ViewModelBase Navigate<T>(this INavigationService navigationService, object param) where T : ViewModelBase
        {
            var viewModel = ViewModelLocator.Get<T>();
            return navigationService.Navigate(param, viewModel);
        }

        public static ViewModelBase Navigate(this INavigationService navigationService, object param, ViewModelBase viewModel)
        {
            viewModel.Initialise(param);
            navigationService.NavigateTo(viewModel.Key, param);
            return viewModel;
        }
    }
}