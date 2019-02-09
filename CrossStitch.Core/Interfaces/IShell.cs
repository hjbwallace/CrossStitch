using CrossStitch.Core.ViewModels;
using System.ComponentModel;

namespace CrossStitch.Core.Interfaces
{
    public interface IShell : INotifyPropertyChanged
    {
        string Page { get; }

        void Back();

        ViewModelBase Navigate<T>() where T : ViewModelBase;

        ViewModelBase Navigate<T>(object param) where T : ViewModelBase;
    }
}