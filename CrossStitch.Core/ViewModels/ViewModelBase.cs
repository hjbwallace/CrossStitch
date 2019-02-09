using CrossStitch.Core.Interfaces;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using System.ComponentModel;
using System.Windows.Input;

namespace CrossStitch.Core.ViewModels
{
    public abstract class ViewModelBase : GalaSoft.MvvmLight.ViewModelBase, INotifyPropertyChanged
    {
        private readonly IShell _shell;

        public ViewModelBase()
            : this(SimpleIoc.Default.GetInstance<IShell>())
        {
        }

        protected ViewModelBase(IShell shell)
        {
            _shell = shell;
            Key = GetType().Name.Replace("ViewModel", "View");

            CancelCommand = new RelayCommand(OnCancel);
        }

        public ICommand CancelCommand { get; protected set; }

        public string Key { get; protected set; }

        public abstract void Initialise(object param);

        public virtual void OnBack()
        {
        }

        protected void GoBack()
        {
            _shell.Back();
        }

        protected void Navigate<T>(object param = null) where T : ViewModelBase
        {
            _shell.Navigate<T>(param);
        }

        protected virtual void OnCancel() => GoBack();
    }
}