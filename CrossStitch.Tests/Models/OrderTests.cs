using CrossStitch.Core.Models;
using FluentAssertions;
using System;
using Xunit;

namespace CrossStitch.Tests.Models
{
    public class OrderTests
    {
        public Order Order { get; set; }

        [Theory]
        [InlineData(OrderState.AwaitingConfirmation)]
        [InlineData(OrderState.Created)]
        private void CanCancelOrderFromValidState(OrderState initialState)
        {
            var expected = new
            {
                DateCompleted = DateTime.Now,
                Status = OrderState.Cancelled
            };

            var order = new Order();
            order.SetProperty(a => a.Status, initialState);
            order.Cancel(expected.DateCompleted);

            order.Should().BeEquivalentTo(expected);
        }

        [Fact]
        private void CanCompleteOrderFromAwaitingState()
        {
            var expected = new
            {
                DateCreated = new DateTime(),
                DateOrdered = (DateTime?)null,
                DateCompleted = DateTime.Now,
                Status = OrderState.Completed
            };

            var order = new Order();
            order.SetProperty(a => a.Status, OrderState.AwaitingConfirmation);
            order.Complete(expected.DateCompleted);

            order.Should().BeEquivalentTo(expected);
        }

        [Fact]
        private void CanCreateOrderFromNoneState()
        {
            var expected = new
            {
                DateCreated = DateTime.Now,
                DateOrdered = (DateTime?)null,
                DateCompleted = (DateTime?)null,
                Status = OrderState.Created
            };

            var order = new Order();
            order.SetProperty(a => a.Status, OrderState.None);
            order.Create(expected.DateCreated);

            order.Should().BeEquivalentTo(expected);
        }

        [Fact]
        private void CanSendOrderFromCreatedState()
        {
            var expected = new
            {
                DateCreated = new DateTime(),
                DateOrdered = DateTime.Now,
                DateCompleted = (DateTime?)null,
                Status = OrderState.AwaitingConfirmation
            };

            var order = new Order();
            order.SetProperty(a => a.Status, OrderState.Created);
            order.Send(expected.DateOrdered);

            order.Should().BeEquivalentTo(expected);
        }

        [Fact]
        private void CanSendOrderFromNoneState()
        {
            var now = DateTime.Now;
            var expected = new
            {
                DateCreated = now,
                DateOrdered = now,
                DateCompleted = (DateTime?)null,
                Status = OrderState.AwaitingConfirmation
            };

            var order = new Order();
            order.SetProperty(a => a.Status, OrderState.None);
            order.Send(now);

            order.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(OrderState.None)]
        [InlineData(OrderState.Completed)]
        [InlineData(OrderState.Cancelled)]
        private void ExceptionWhenCancellingOrderFromInvalidState(OrderState initialState)
        {
            var order = new Order();
            order.SetProperty(a => a.Status, initialState);

            Action cancelAction = () => order.Cancel(DateTime.Now);
            cancelAction.Should().Throw<InvalidOperationException>();
        }

        [Theory]
        [InlineData(OrderState.None)]
        [InlineData(OrderState.Created)]
        [InlineData(OrderState.Completed)]
        [InlineData(OrderState.Cancelled)]
        private void ExceptionWhenCompletingOrderFromInvalidState(OrderState initialState)
        {
            var order = new Order();
            order.SetProperty(a => a.Status, initialState);

            Action completeAction = () => order.Complete(DateTime.Now);
            completeAction.Should().Throw<InvalidOperationException>();
        }

        [Theory]
        [InlineData(OrderState.Created)]
        [InlineData(OrderState.AwaitingConfirmation)]
        [InlineData(OrderState.Completed)]
        [InlineData(OrderState.Cancelled)]
        private void ExceptionWhenCreatingOrderFromInvalidState(OrderState initialState)
        {
            var order = new Order();
            order.SetProperty(a => a.Status, initialState);

            Action createAction = () => order.Create(DateTime.Now);
            createAction.Should().Throw<InvalidOperationException>();
        }

        [Theory]
        [InlineData(OrderState.AwaitingConfirmation)]
        [InlineData(OrderState.Completed)]
        [InlineData(OrderState.Cancelled)]
        private void ExceptionWhenSendingOrderFromInvalidState(OrderState initialState)
        {
            var order = new Order();
            order.SetProperty(a => a.Status, initialState);

            Action sendAction = () => order.Send(DateTime.Now);
            sendAction.Should().Throw<InvalidOperationException>();
        }
    }
}