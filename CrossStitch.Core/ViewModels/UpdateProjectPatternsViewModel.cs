using CrossStitch.Core.Attributes;
using CrossStitch.Core.Data;
using CrossStitch.Core.Extensions;
using CrossStitch.Core.Models;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace CrossStitch.Core.ViewModels
{
    [Title("Update Project Patterns")]
    public class UpdateProjectPatternsViewModel : ViewModelBase
    {
        public Pattern[] _patterns;
        private readonly PatternRepository _patternRepository;

        public UpdateProjectPatternsViewModel(PatternRepository patternRepository)
        {
            _patternRepository = patternRepository;

            ConfirmCommand = new RelayCommand(OnConfirm);
        }

        public ICommand ConfirmCommand { get; }

        public IEnumerable<Pattern> Patterns { get; set; } = new List<Pattern>();

        public PatternSelection PatternSelectionDto { get; set; }

        public ObservableCollection<SelectionWrapper<Pattern>> PatternSelections { get; set; }

        public Project Project { get; set; }

        public override void Initialise(object param)
        {
            if (!(param is PatternSelection patternSelectionDto))
                throw new ArgumentException("Must pass in a valid project to manage");

            PatternSelectionDto = patternSelectionDto;
            Patterns = _patternRepository.GetAll();

            PatternSelections = Patterns.Select(o => new SelectionWrapper<Pattern>(o)
            {
                IsSelected = PatternSelectionDto.Patterns.Any(a => a.PatternId == o.PatternId),
            }).AsObservable();
        }

        private void OnConfirm()
        {
            PatternSelectionDto.Patterns = PatternSelections.Where(x => x.IsSelected).Select(x => x.Wrapped).ToArray();
            PatternSelectionDto.IsUpdated = true;
            GoBack();
        }
    }
}