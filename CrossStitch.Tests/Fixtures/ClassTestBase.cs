using CrossStitch.Core.ViewModels;

namespace CrossStitch.Tests.Fixtures
{
    public abstract class ClassTestBase<T> : DatabaseTestBase
    {
        public ClassTestBase()
        {
            UnderTest = Instance<T>();
        }

        public T UnderTest { get; }
    }

    public class ViewModelTestBase<TViewModel> : ClassTestBase<TViewModel> where TViewModel : ViewModelBase
    {
        public ViewModelTestBase(object param = null)
        {
            Shell.Navigate<TViewModel>(param);
        }

        public void NavigatesTo<T>(object param) where T : ViewModelBase
        {
            Shell.CurrentViewModelIs<T>();
            Shell.PreviousInitialiserWas(param);
        }
    }
}