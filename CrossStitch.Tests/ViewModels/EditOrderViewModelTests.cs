using CrossStitch.Core.Data;
using CrossStitch.Core.Extensions;
using CrossStitch.Core.Models;
using CrossStitch.Core.ViewModels;
using CrossStitch.Tests.CommonActions;
using CrossStitch.Tests.Fixtures;
using CrossStitch.Tests.Mocks;
using FluentAssertions;
using GalaSoft.MvvmLight.Ioc;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CrossStitch.Tests.ViewModels
{
    public class EditOrderViewModelTests : ViewModelTestBase<EditOrderViewModel>
    {
        private readonly OrderRepository _orderRepository;
        private readonly ThreadRepository _threadRepository;

        public EditOrderViewModelTests() : base(new Order())
        {
            _threadRepository = SimpleIoc.Default.GetInstance<ThreadRepository>();
            _orderRepository = SimpleIoc.Default.GetInstance<OrderRepository>();

            _orderRepository.Clear();
            _threadRepository.Clear();
        }

        [Theory]
        [InlineData("", "", false)]
        [InlineData("Some Id", null, false)]
        [InlineData(null, "Some Brand", false)]
        [InlineData("Some Id", "Some Brand", true)]
        public void CanAddThreadCommand(string brandId, string brandName, bool expected)
        {
            UnderTest.AddThreadModel = new AddThreadModel
            {
                BrandId = brandId,
                BrandName = brandName
            };

            UnderTest.AddThreadCommand.CanExecute(null).Should().Be(expected);
        }

        [Theory]
        [InlineData(OrderState.None, true)]
        [InlineData(OrderState.Created, true)]
        [InlineData(OrderState.AwaitingConfirmation, false)]
        [InlineData(OrderState.Completed, false)]
        [InlineData(OrderState.Cancelled, false)]
        public void CanEditOrderBasedOnStatus(OrderState status, bool expectedCanEdit)
        {
            UnderTest.Model.SetProperty(a => a.Status, status);
            var canEdit = UnderTest.CanEdit;

            canEdit.Should().Be(expectedCanEdit);
        }

        [Theory]
        [InlineData(123, 2, 100, true)]
        [InlineData(0, 1, 2, false)]
        [InlineData(-1, 1, 2, false)]
        [InlineData(null, 1, 2, false)]
        public void CanSaveOrderWithMultipleThreadLength(int? length1, int? length2, int? length3, bool expected)
        {
            UnderTest.Model.Threads = new List<OrderThread>
            {
                new OrderThread { Length = length1 },
                new OrderThread { Length = length2 },
                new OrderThread { Length = length3 },
            };

            var canSave = UnderTest.SaveCommand.CanExecute(null);
            canSave.Should().Be(expected);
        }

        [Theory]
        [InlineData(123, true)]
        [InlineData(0, false)]
        [InlineData(-1, false)]
        [InlineData(null, false)]
        public void CanSaveOrderWithThreadLength(int? length, bool expected)
        {
            UnderTest.Model.Threads = new OrderThread { Length = length }.AsArray();

            var canSave = UnderTest.SaveCommand.CanExecute(null);
            canSave.Should().Be(expected);
        }

        [Fact]
        public void DeleteThreadFromDatabase()
        {
            var order = UnderTest.Model;
            UpdateOrder(order);

            var expectedThread = new
            {
                Length = 2,
                ThreadReferenceId = 1
            };

            var thread = new OrderThread
            {
                Length = expectedThread.Length,
                ThreadReferenceId = expectedThread.ThreadReferenceId
            };

            order.Threads = thread.AsList();
            UnderTest.SaveCommand.Execute(null);

            HasEntries<Order>(1);
            HasEntries<ThreadBase>(1);

            UnderTest.DeleteThreadCommand.Execute(thread);
            UnderTest.SaveCommand.Execute(null);

            HasEntries<Order>(1);
            HasEntries<ThreadBase>(0);
        }

        [Fact]
        public void DeleteThreadFromOrder()
        {
            var orderThread = new OrderThread
            {
                ThreadReference = ThreadReferences.Data[0]
            };

            var order = UnderTest.Model;

            order.Threads = orderThread.AsList();

            UnderTest.DeleteThreadCommand.Execute(orderThread);

            order.Threads.Should().BeEmpty();
        }

        [Fact]
        public void DialogShownWhenValidatingModel()
        {
            var order = ValidOrder();
            order.Description = null;

            UnderTest.Model = order;

            UnderTest.SaveCommand.Execute(null);
            Dialog.LastDialogWas(DialogType.Warning, "Validation Issues");

            var orders = _orderRepository.GetAll();
            orders.Should().BeEmpty();

            HasEntries<Order>(0);
        }

        [Theory]
        [InlineData("1", "Other Brand")]
        public void InformationDialogShownWhenAddingDuplicateThread(string brandId, string brandName)
        {
            UnderTest.Model.Threads = new OrderThread
            {
                ThreadReference = new ThreadReference
                {
                    BrandId = brandId,
                    BrandName = brandName
                }
            }.AsArray();

            UnderTest.AddThreadModel = new AddThreadModel
            {
                BrandId = brandId,
                BrandName = brandName
            };

            UnderTest.AddThreadCommand.Execute(null);
            Dialog.LastDialogWas(DialogType.Info, "Duplicate Thread Reference");
        }

        [Theory]
        [InlineData("22", "Other Brand")]
        public void InformationDialogShownWhenCannotFindThreadToAdd(string brandId, string brandName)
        {
            UnderTest.AddThreadModel = new AddThreadModel
            {
                BrandId = brandId,
                BrandName = brandName
            };

            UnderTest.AddThreadCommand.Execute(null);
            Dialog.LastDialogWas(DialogType.Info, "Missing Thread Reference");
        }

        [Fact]
        public void SavingNewOrder()
        {
            var order = UnderTest.Model;
            UpdateOrder(order);

            UnderTest.SaveCommand.Execute(null);
            Dialog.NoDialogShown();

            var orders = _orderRepository.GetAll();
            orders.Last().Should().BeEquivalentTo(order);

            HasEntries<Order>(1);
        }

        [Fact]
        public void SavingNewOrderWithThread()
        {
            var order = UnderTest.Model;
            UpdateOrder(order);

            var expectedThread = new
            {
                Length = 2,
                ThreadReferenceId = 1
            };

            var thread = new OrderThread
            {
                Length = expectedThread.Length,
                ThreadReferenceId = expectedThread.ThreadReferenceId
            };

            order.Threads = thread.AsList();
            UnderTest.SaveCommand.Execute(null);

            var dbOrder = _orderRepository.GetAll().Last();
            dbOrder.Should().BeEquivalentTo(order, opt => opt.Excluding(o => o.Threads));
            dbOrder.Threads.Should().HaveCount(1);
            dbOrder.Threads.Should().BeEquivalentTo(expectedThread);

            HasEntries<Order>(1);
            HasEntries<ThreadBase>(1);
        }

        private static void UpdateOrder(Order order)
        {
            order.OrderId = default(int);
            order.Store = "Some Store";
            order.Description = "Some Description";
        }

        private static Order ValidOrder() => new Order
        {
            Store = "Some Store",
            Description = "Some Description",
            Threads = ValidThread().AsList(),
        };

        private static OrderThread ValidThread() => new OrderThread
        {
            Length = 5,
        };

        // private void CanCancelAnOrderThatWasSent()
        // private void CanCancelAnOrderThatWasCreated()

        // private void CanCompleteAnOrderThatWasSent()
        // private void CanSendAnOrder(OrderState state) // for None and Created
    }
}