using CrossStitch.Core.Attributes;
using CrossStitch.Core.Data;
using CrossStitch.Core.Extensions;
using CrossStitch.Core.Interfaces;
using CrossStitch.Core.Models;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace CrossStitch.Core.ViewModels
{
    [Title("Manage Projects")]
    public class ManageProjectsViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogService;

        private readonly ProjectRepository _projectRepository;

        public ManageProjectsViewModel(INavigationService navigationService, ProjectRepository projectRepository, IDialogService dialogService)
        {
            _projectRepository = projectRepository;
            _dialogService = dialogService;

            EditProjectCommand = new RelayCommand<Project>(OnEditProject);
            NewProjectCommand = new RelayCommand(OnNewProject);
            RefreshCommand = new RelayCommand(OnRefresh);
            DeleteProjectCommand = new RelayCommand<Project>(OnDeleteProject);
        }

        public ICommand DeleteProjectCommand { get; }

        public ICommand EditProjectCommand { get; }

        public ICommand NewProjectCommand { get; }

        public ObservableCollection<Project> Projects { get; set; } = new ObservableCollection<Project>();

        public ICommand RefreshCommand { get; }

        public Project SelectedItem { get; set; }

        public override void Initialise(object param)
        {
            OnRefresh();
        }

        public override void OnBack() => OnRefresh();

        private void ManageProject(Project project)
        {
            Navigate<EditProjectViewModel>(project);
        }

        private void OnDeleteProject(Project project)
        {
            if (!_dialogService.ShowQuestion($"Are you sure you want to delete project \"{project.Name}\"", "Confirm Project Deletion"))
                return;

            _projectRepository.Remove(project);
        }

        private void OnEditProject(Project project) => ManageProject(project);

        private void OnNewProject() => ManageProject(new Project());

        private void OnRefresh()
        {
            Projects = _projectRepository.GetAll().OrderBy(o => o.Name).AsObservable();
        }
    }
}