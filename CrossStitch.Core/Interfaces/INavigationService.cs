using System;

namespace CrossStitch.Core.Interfaces
{
    public interface INavigationService
    {
        void Configure(string key, Type pageType);

        void GoBack();

        void NavigateTo(string pageKey);

        void NavigateTo(string pageKey, object parameter);
    }
}