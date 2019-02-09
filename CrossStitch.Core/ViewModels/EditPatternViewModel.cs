using CrossStitch.Core.Attributes;
using CrossStitch.Core.Data;
using CrossStitch.Core.Interfaces;
using CrossStitch.Core.Models;
using GalaSoft.MvvmLight.CommandWpf;
using System.Linq;
using System.Windows.Input;

namespace CrossStitch.Core.ViewModels
{
    [Title("Edit Pattern")]
    public class EditPatternViewModel : EditEntityWithThreadsViewModel<Pattern, PatternThread>
    {
        private readonly PatternRepository _patternRepository;

        public EditPatternViewModel(PatternRepository patternRepository,
            ThreadReferenceRepository threadReferenceRepository,
            ThreadRepository threadRepository,
            IDialogService dialogService)
            : base(threadReferenceRepository, threadRepository, dialogService)
        {
            _patternRepository = patternRepository;

            GenerateOrderCommand = new RelayCommand(OnGenerateOrder, CanGenerateOrder);
        }

        public ICommand GenerateOrderCommand { get; }

        protected override PatternThread GenerateThread(ThreadReference threadReference)
        {
            return new PatternThread
            {
                Pattern = Model,
                ThreadReference = threadReference,
                ThreadReferenceId = threadReference.Id,
            };
        }

        protected override void SaveModel(Pattern entity)
        {
            _patternRepository.Save(entity);
        }

        private bool CanGenerateOrder()
        {
            return Model.Threads.Any() && CanSave();
        }

        private void OnGenerateOrder()
        {
            var order = new Order
            {
                Description = "Order for " + Model.Name,
            };

            order.Threads = Model.Threads.Select(ts => new OrderThread
            {
                ThreadReference = ts.ThreadReference,
                ThreadReferenceId = ts.ThreadReference?.Id ?? -1,
                Order = order,
                Length = ts.Length
            }).ToArray();

            Navigate<EditOrderViewModel>(order);
        }
    }
}