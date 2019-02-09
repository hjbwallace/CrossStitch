using CrossStitch.Core.Interfaces;
using CrossStitch.Core.ViewModels;
using FluentAssertions;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CrossStitch.Tests.Mocks
{
    public class TestShell : IShell
    {
        private readonly INavigationService _navigationService;
        private object _previousInitialiser;
        private Stack<ViewModelBase> _viewModels = new Stack<ViewModelBase>();

        public TestShell(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModelBase CurrentViewModel => _viewModels.Peek();
        public string Page => throw new NotImplementedException();

        public void Back()
        {
            if (_viewModels.Any())
                _viewModels.Pop();

            if (_viewModels.Any())
                CurrentViewModel.OnBack();
        }

        public void CurrentViewModelIs<T>()
        {
            CurrentViewModel.GetType().Should().Be<T>();
        }

        public ViewModelBase Navigate<T>() where T : ViewModelBase
        {
            return Navigate<T>(null);
        }

        public ViewModelBase Navigate<T>(object param) where T : ViewModelBase
        {
            var viewModel = SimpleIoc.Default.GetInstance<T>();
            viewModel.Initialise(param);
            _viewModels.Push(viewModel);
            _previousInitialiser = param;
            return viewModel;
        }

        public void PreviousInitialiserWas(object expected)
        {
            _previousInitialiser.Should().BeEquivalentTo(expected);
        }
    }
}