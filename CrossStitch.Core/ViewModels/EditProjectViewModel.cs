using CrossStitch.Core.Attributes;
using CrossStitch.Core.Data;
using CrossStitch.Core.Extensions;
using CrossStitch.Core.Interfaces;
using CrossStitch.Core.Models;
using GalaSoft.MvvmLight.CommandWpf;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Input;

namespace CrossStitch.Core.ViewModels
{
    [Title("Edit Project")]
    public class EditProjectViewModel : ViewModelBase
    {
        private readonly ICurrentDateService _dateTimeProvider;
        private readonly IDialogService _dialogService;
        private readonly PatternProjectRepository _patternProjectRepository;
        private readonly PatternRepository _patternRepository;
        private readonly ProjectRepository _projectRepository;
        private readonly ThreadRepository _threadRepository;
        private int[] _initialIds;

        private PatternProject[] _initialPatternProjects;

        public EditProjectViewModel(INavigationService navigationService,
                            PatternRepository patternRepository,
            ProjectRepository projectRepository,
            IDialogService dialogService,
            PatternProjectRepository patternProjectRepository,
            ThreadRepository threadRepository,
            ICurrentDateService dateTimeProvider)
        {
            SaveCommand = new RelayCommand(OnSave);
            UpdatePatternsCommand = new RelayCommand<Project>(OnUpdatePatterns);
            GenerateOrderCommand = new RelayCommand(OnGenerateOrder, CanGenerateOrder);
            _patternRepository = patternRepository;
            _projectRepository = projectRepository;
            _dialogService = dialogService;
            _patternProjectRepository = patternProjectRepository;
            _threadRepository = threadRepository;
            _dateTimeProvider = dateTimeProvider;

            PatternSelectionDto = new PatternSelection();
        }

        public Pattern[] _currentPatterns { get; set; }

        public ICommand GenerateOrderCommand { get; }

        public bool IsNew { get; set; } = true;

        public IEnumerable<Pattern> Patterns { get; set; } = new List<Pattern>();

        public PatternSelection PatternSelectionDto { get; set; }

        public ObservableCollection<SelectionWrapper<Pattern>> PatternSelections { get; set; }

        public Project Project { get; set; } = new Project();

        public ICommand SaveCommand { get; }

        public ProjectThreadSummary[] ThreadSummary { get; set; } = new ProjectThreadSummary[0];

        public ICommand UpdatePatternsCommand { get; }

        private Dictionary<int, PatternProject> _removedPatternProjects { get; set; } = new Dictionary<int, PatternProject>();

        public override void Initialise(object param)
        {
            Patterns = _patternRepository.GetAll();

            Project = (param as Project) ?? new Project();
            IsNew = Project == new Project();

            _removedPatternProjects = new Dictionary<int, PatternProject>();

            _initialPatternProjects = Project.PatternProjects.ToArray();

            PatternSelections = Patterns.Select(o => new SelectionWrapper<Pattern>(o)
            {
                IsSelected = Project.PatternProjects.Any(a => a.PatternId == o.PatternId),
            }).AsObservable();

            _initialIds = Project.PatternProjects.Select(o => o.PatternProjectId).ToArray();

            UpdateThreadSummary();
        }

        public override void OnBack()
        {
            if (!PatternSelectionDto.IsUpdated)
                return;

            var newPatterns = PatternSelectionDto.Patterns
                .Where(ps => !Project.PatternProjects.Any(pp => pp.PatternId == ps.PatternId))
                .ToArray();

            var removedPatterns = Project.PatternProjects
                .Where(ps => !PatternSelectionDto.Patterns.Any(psd => ps.Pattern != null && psd.PatternId == ps.Pattern.PatternId))
                .ToArray();

            foreach (var removedPattern in removedPatterns.Where(rp => !rp.IsDefault()))
            {
                _removedPatternProjects[removedPattern.PatternProjectId] = removedPattern;
            }

            foreach (var pattern in newPatterns)
            {
                var toAdd = new PatternProject
                {
                    Pattern = pattern,
                    PatternId = pattern.PatternId,
                    Project = Project,
                    ProjectId = Project.ProjectId,
                };

                Project.PatternProjects.Add(toAdd);
            }

            foreach (var pattern in removedPatterns)
            {
                var toRemove = Project.PatternProjects.Single(pp => pp.PatternId == pattern.PatternId);
                Project.PatternProjects.Remove(toRemove);
            }

            UpdateThreadSummary();
        }

        private bool CanGenerateOrder()
        {
            return ThreadSummary.Any();
        }

        private void OnGenerateOrder()
        {
            var order = new Order
            {
                Description = "Order for " + Project.Name,
            };

            order.Threads = ThreadSummary.Select(ts => new OrderThread
            {
                ThreadReference = ts.ThreadReference,
                ThreadReferenceId = ts.ThreadReference.Id,
                Order = order,
                Length = ts.RequiredLength
            }).ToArray();

            Navigate<EditOrderViewModel>(order);
        }

        private void OnSave()
        {
            Log.Information("Attempting to save {Type}", typeof(Project).Name);

            var context = new ValidationContext(Project, null, null);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(Project, context, results, true);

            if (results.Any())
            {
                var message = "Couldn't save the project for the following reason(s):\n" + string.Join("\n", results);
                _dialogService.ShowWarning(message, "Validation Issues");
                return;
            }

            var currentPatterns = Project.PatternProjects.Select(x => x.PatternProjectId);
            var removedPatterns = _initialIds.Except(currentPatterns);

            _patternProjectRepository.Remove(_removedPatternProjects.Values.ToArray());

            Project.Create(_dateTimeProvider.Now());

            Project.PatternProjects = Project.PatternProjects.Select(x =>
            {
                x.Pattern = null;
                return x;
            }).AsObservable();
            _projectRepository.Save(Project);

            GoBack();
        }

        private void OnUpdatePatterns(Project project)
        {
            _currentPatterns = Project.PatternProjects.Select(x => x.Pattern).ToArray();
            PatternSelectionDto = new PatternSelection
            {
                Patterns = Project.PatternProjects.Select(x => x.Pattern).ToArray(),
                ProjectName = Project.Name,
            };
            Navigate<UpdateProjectPatternsViewModel>(PatternSelectionDto);
        }

        private void UpdateThreadSummary()
        {
            ThreadSummary = Project.PatternProjects
                .Where(pp => !pp.IsCompleted)
                .SelectMany(pp => pp.Pattern.Threads)
                .GroupBy(pp => pp.ThreadReference)
                .Select(x => new ProjectThreadSummary(x.Key)
                {
                    RequiredLength = x.Sum(e => e.Length ?? 0)
                })
                .ToArray();
        }
    }

    public class ProjectThreadSummary : NotifyPropertyChanged
    {
        public ProjectThreadSummary(ThreadReference threadReference)
        {
            ThreadReference = threadReference ?? throw new ArgumentException("Thread reference must be populated");
        }

        public int RequiredLength { get; set; }
        public ThreadReference ThreadReference { get; }
    }
}