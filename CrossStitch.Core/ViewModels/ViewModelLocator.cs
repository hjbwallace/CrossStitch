using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;

namespace CrossStitch.Core.ViewModels
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
        }

        public EditOrderViewModel EditOrderView => Get<EditOrderViewModel>();

        public EditPatternViewModel EditPatternView => Get<EditPatternViewModel>();
        public EditProjectViewModel EditProjectView => Get<EditProjectViewModel>();
        public EditThreadReferenceViewModel EditThreadReferenceView => Get<EditThreadReferenceViewModel>();
        public HomeViewModel HomeView => Get<HomeViewModel>();
        public ManageInventoryViewModel ManageInventoryView => Get<ManageInventoryViewModel>();
        public ManageOrdersViewModel ManageOrdersView => Get<ManageOrdersViewModel>();
        public ManagePatternsViewModel ManagePatternsView => Get<ManagePatternsViewModel>();
        public ManageProjectsViewModel ManageProjectsView => Get<ManageProjectsViewModel>();
        public UpdateProjectPatternsViewModel UpdateProjectPatternsView => Get<UpdateProjectPatternsViewModel>();

        public static T Get<T>() where T : ViewModelBase
        {
            return SimpleIoc.Default.GetInstance<T>();
        }
    }
}