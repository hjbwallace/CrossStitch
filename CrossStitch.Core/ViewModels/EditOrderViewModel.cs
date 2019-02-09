using CrossStitch.Core.Attributes;
using CrossStitch.Core.Data;
using CrossStitch.Core.Interfaces;
using CrossStitch.Core.Models;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Linq;
using System.Windows.Input;

namespace CrossStitch.Core.ViewModels
{
    [Title("Edit Order")]
    public class EditOrderViewModel : EditEntityWithThreadsViewModel<Order, OrderThread>
    {
        private readonly ICurrentDateService _dateTimeProvider;
        private readonly OrderRepository _orderRepository;

        public EditOrderViewModel(
            OrderRepository orderRepository,
            ThreadReferenceRepository threadReferenceRepository,
            ThreadRepository threadRepository,
            IDialogService dialogService,
            ICurrentDateService dateTimeProvider)
            : base(threadReferenceRepository, threadRepository, dialogService)
        {
            _orderRepository = orderRepository;
            _dateTimeProvider = dateTimeProvider;
            CompleteOrderCommand = new RelayCommand(OnCompleteOrder);
            CancelOrderCommand = new RelayCommand(OnCancelOrder, CanCancel);
            MakeOrderCommand = new RelayCommand(OnMakeOrder, CanMakeOrder);
        }

        public ICommand CancelOrderCommand { get; }
        public bool CanEdit => Model.Status == OrderState.Created || Model.Status == OrderState.None;
        public ICommand CompleteOrderCommand { get; }
        public bool IsComplete => Model.Status == OrderState.Completed || Model.Status == OrderState.Cancelled;
        public bool IsReadOnly => Model.Status != OrderState.Created;
        public ICommand MakeOrderCommand { get; }

        public override void Initialise(object param)
        {
            base.Initialise(param);
            RaisePropertyChanged(() => CanEdit);
        }

        protected override bool CanDeleteThread(OrderThread thread) => CanEdit;

        protected override bool CanSave()
        {
            return base.CanSave() && CanEdit;
        }

        protected override OrderThread GenerateThread(ThreadReference threadReference)
        {
            return new OrderThread
            {
                Order = Model,
                ThreadReference = threadReference,
                ThreadReferenceId = threadReference.Id,
            };
        }

        protected override void SaveModel(Order entity)
        {
            if (entity.DateCreated == new DateTime())
                entity.Create(_dateTimeProvider.Now());

            _orderRepository.Save(entity);
        }

        private bool CanCancel()
        {
            return Model.Status == OrderState.Created || Model.Status == OrderState.AwaitingConfirmation;
        }

        private bool CanMakeOrder()
        {
            return Model.Threads.Any() && CanSave();
        }

        private void OnCancelOrder()
        {
            Model.Cancel(_dateTimeProvider.Now());
            SaveModel(Model);
            RaisePropertyChanged(() => CanEdit);
        }

        private void OnCompleteOrder()
        {
            Model.Complete(_dateTimeProvider.Now());

            var updateInventory = _dialogService.ShowQuestion("Would you like to update the inventory with the following values?", "Update Inventory");

            if (updateInventory)
                UpdateInventory();

            SaveModel(Model);
        }

        private void OnMakeOrder()
        {
            Model.Send(_dateTimeProvider.Now());
            SaveModel(Model);
            RaisePropertyChanged(() => CanEdit);
        }

        private void UpdateInventory()
        {
            foreach (var thread in Model.Threads)
            {
                var threadReference = thread.ThreadReference;
                threadReference.OwnedLength += thread.Length.GetValueOrDefault();
            }

            var othersToUpdate = Model.Threads.Select(t => t.ThreadReference).ToArray();

            _threadReferenceRepository.Save(othersToUpdate);
        }
    }
}