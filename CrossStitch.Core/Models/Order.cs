using CrossStitch.Core.Attributes;
using CrossStitch.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace CrossStitch.Core.Models
{
    public enum OrderState
    {
        [Ui("")] None,
        [Ui("Created")] Created,
        [Ui("Awaiting Confirmation")] AwaitingConfirmation,
        [Ui("Completed")] Completed,
        [Ui("Cancelled")] Cancelled
    }

    public class Order : NotifyPropertyChanged, IEntityWithStatus<OrderState>, IEntityWithThreads<OrderThread>
    {
        public Order()
        {
            Threads = new ObservableCollection<OrderThread>();
        }

        [Ui("Date Completed")]
        public DateTime? DateCompleted { get; private set; }

        [Ui("Date Ordered")]
        public DateTime? DateOrdered { get; private set; }

        [Ui("Date Created")]
        public DateTime DateCreated { get; private set; }

        [Required]
        [Ui("Description")]
        public string Description { get; set; }

        [Key]
        public int OrderId { get; set; }

        public OrderState Status { get; private set; }

        public StatusType StatusType
        {
            get
            {
                switch (Status)
                {
                    case OrderState.Created:
                    case OrderState.AwaitingConfirmation:
                        return StatusType.RequiresAttention;

                    case OrderState.Completed:
                        return StatusType.Good;

                    case OrderState.Cancelled:
                        return StatusType.Bad;

                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        [Required]
        [Ui("Store")]
        public string Store { get; set; }

        public ICollection<OrderThread> Threads { get; set; }

        public void Create(DateTime now)
        {
            if (Status != OrderState.None)
                InvalidStateTransitionTo(OrderState.Created);

            Status = OrderState.Created;
            DateCreated = now;
        }

        public void Cancel(DateTime now)
        {
            if (Status != OrderState.Created && Status != OrderState.AwaitingConfirmation)
                InvalidStateTransitionTo(OrderState.Cancelled);

            Status = OrderState.Cancelled;
            DateCompleted = now;
        }

        public void Complete(DateTime now)
        {
            if (Status != OrderState.AwaitingConfirmation)
                InvalidStateTransitionTo(OrderState.Completed);

            Status = OrderState.Completed;
            DateCompleted = now;
        }

        public void Send(DateTime now)
        {
            if (Status == OrderState.None)
                Create(now);

            if (Status != OrderState.Created)
                InvalidStateTransitionTo(OrderState.AwaitingConfirmation);

            Status = OrderState.AwaitingConfirmation;
            DateOrdered = now;
        }

        private void InvalidStateTransitionTo(OrderState state)
        {
            throw new InvalidOperationException(
                $"Cannot transition order state from \"{Status}\" to \"{state}\"");
        }
    }
}