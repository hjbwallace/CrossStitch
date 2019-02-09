using CrossStitch.Core.Attributes;
using CrossStitch.Core.Interfaces;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows.Input;

namespace CrossStitch.Core.ViewModels
{
    [Title("Cross Stitch")]
    public class HomeViewModel : ViewModelBase
    {
        private readonly IBackupService _backupService;
        private readonly IDatabaseContextService _contextFactory;
        private readonly ICurrentDateService _dateTimeProvider;

        public HomeViewModel(IDatabaseContextService contextFactory, ICurrentDateService dateTimeProvider, IBackupService backupService)
        {
            _contextFactory = contextFactory;
            _dateTimeProvider = dateTimeProvider;
            _backupService = backupService;

            // Warm up the terminal
            contextFactory.GetContext().Invoke();

            ManagePatternsCommand = new RelayCommand(OnManagePatterns);
            ManageProjectsCommand = new RelayCommand(OnManageProjects);
            ManageOrdersCommand = new RelayCommand(OnManageOrders);
            ManageInventoryCommand = new RelayCommand(OnManageInventory);
            BackupDatabaseCommand = new RelayCommand(OnBackupDatabase);
        }

        public ICommand BackupDatabaseCommand { get; }
        public ICommand ManageInventoryCommand { get; }
        public ICommand ManageOrdersCommand { get; }
        public ICommand ManagePatternsCommand { get; }
        public ICommand ManageProjectsCommand { get; }

        public override void Initialise(object param)
        {
        }

        private void OnBackupDatabase() => _backupService.BackupDatabase();

        private void OnManageInventory() => Navigate<ManageInventoryViewModel>();

        private void OnManageOrders() => Navigate<ManageOrdersViewModel>();

        private void OnManagePatterns() => Navigate<ManagePatternsViewModel>();

        private void OnManageProjects() => Navigate<ManageProjectsViewModel>();
    }
}