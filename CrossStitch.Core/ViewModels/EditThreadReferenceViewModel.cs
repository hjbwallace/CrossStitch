using CrossStitch.Core.Attributes;
using CrossStitch.Core.Data;
using CrossStitch.Core.Models;
using CrossStitch.Core.Validation;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows.Input;

namespace CrossStitch.Core.ViewModels
{
    [Title("Edit Thread Reference")]
    public class EditThreadReferenceViewModel : ViewModelBase
    {
        private readonly ThreadReferenceRepository _threadReferenceRepository;

        public EditThreadReferenceViewModel(ThreadReferenceRepository threadReferenceRepository)
        {
            _threadReferenceRepository = threadReferenceRepository;
            SaveCommand = new RelayCommand(OnSave, CanSave);
        }

        public ICommand SaveCommand { get; }
        public ThreadReference ThreadReference { get; set; } = new ThreadReference();

        public override void Initialise(object param)
        {
            ThreadReference = (param as ThreadReference) ?? new ThreadReference();
        }

        private bool CanSave() => CustomValidator.TryValidate(ThreadReference);

        private void OnSave()
        {
            _threadReferenceRepository.Save(ThreadReference);
            GoBack();
        }
    }
}