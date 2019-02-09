using CrossStitch.Core.Interfaces;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Concurrent;

namespace CrossStitch.Core.Services
{
    public abstract class NavigationServiceBase : INavigationService
    {
        private readonly ConcurrentDictionary<string, Type> _pagesByType = new ConcurrentDictionary<string, Type>();

        public void Configure(string key, Type pageType)
        {
            _pagesByType[key] = pageType;
        }

        public abstract void GoBack();

        public void NavigateTo(string pageKey) => NavigateTo(pageKey, null);

        public virtual void NavigateTo(string pageKey, object parameter)
        {
            if (!_pagesByType.ContainsKey(pageKey))
                throw new ArgumentException($"No such page: {pageKey} ", "pageKey");

            var view = SimpleIoc.Default.GetInstance(_pagesByType[pageKey]);
            PerformNavigation(view);
        }

        protected abstract void PerformNavigation(object view);
    }
}