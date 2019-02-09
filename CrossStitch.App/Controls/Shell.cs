using CrossStitch.Core.Extensions;
using CrossStitch.Core.Interfaces;
using CrossStitch.Core.ViewModels;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace CrossStitch.App.Controls
{
    public class Shell : Frame, IShell
    {
        private readonly INavigationService _navigationService;
        private string _page = "The first name";

        private Stack<ViewModelBase> _viewModels = new Stack<ViewModelBase>();

        public Shell(INavigationService navigationService)
        {
            _navigationService = navigationService;
            DataContext = this;

            BackCommand = new RelayCommand(Back, CanBack);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand BackCommand { get; }
        public ViewModelBase CurrentViewModel => _viewModels.Peek();

        public string Page
        {
            get { return _page; }
            set
            {
                _page = value;
                OnPropertyChanged("Page");
            }
        }

        public void Back()
        {
            _navigationService.GoBack();
            var previousViewModel = _viewModels.Pop();

            CurrentViewModel.OnBack();
            SetPageTitle();
        }

        public ViewModelBase Navigate<T>() where T : ViewModelBase
        {
            return Navigate<T>(null);
        }

        public ViewModelBase Navigate<T>(object param) where T : ViewModelBase
        {
            var viewModel = _navigationService.Navigate<T>(param);
            _viewModels.Push(viewModel);
            SetPageTitle();

            return viewModel;
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private bool CanBack()
        {
            return CanGoBack;
        }

        private void SetPageTitle()
        {
            Page = CurrentViewModel?.GetType().GetTitle() ?? CurrentViewModel.Key;
        }
    }
}