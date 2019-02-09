using CrossStitch.Core.Attributes;
using CrossStitch.Core.Data;
using CrossStitch.Core.Interfaces;
using CrossStitch.Core.Models;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CrossStitch.Core.ViewModels
{
    [Title("Manage Patterns")]
    public class ManagePatternsViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogService;

        private readonly PatternRepository _patternRepository;

        public ManagePatternsViewModel(
                            INavigationService navigationService,
            PatternRepository patternRepository,
            IDialogService dialogService)
        {
            _patternRepository = patternRepository;
            _dialogService = dialogService;

            NewPatternCommand = new RelayCommand(OnNewPattern);
            EditPatternCommand = new RelayCommand<Pattern>(OnPatternEdit);
            DeletePatternCommand = new RelayCommand<Pattern>(OnDeletePattern);
            RefreshCommand = new RelayCommand(PopulatePatterns);
        }

        public ICommand DeletePatternCommand { get; }
        public ICommand EditPatternCommand { get; }
        public ICommand NewPatternCommand { get; }
        public ObservableCollection<Pattern> Patterns { get; set; } = new ObservableCollection<Pattern>();
        public ICommand RefreshCommand { get; }

        public override void Initialise(object param)
        {
            PopulatePatterns();
        }

        private void OnDeletePattern(Pattern pattern)
        {
            if (!_dialogService.ShowQuestion($"Are you sure you want to delete pattern \"{pattern.Name}\"", "Confirm Pattern Deletion"))
                return;

            _patternRepository.Remove(pattern);
        }

        public override void OnBack()
        {
            PopulatePatterns();
        }

        private void OnNewPattern()
        {
            Navigate<EditPatternViewModel>(new Pattern());
        }

        private void OnPatternEdit(Pattern pattern)
        {
            Navigate<EditPatternViewModel>(pattern);
        }

        private void PopulatePatterns()
        {
            var repoPatterns = _patternRepository.GetAll();
            Patterns = new ObservableCollection<Pattern>(repoPatterns);
        }
    }
}