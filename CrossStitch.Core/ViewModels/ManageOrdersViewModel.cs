using CrossStitch.Core.Attributes;
using CrossStitch.Core.Data;
using CrossStitch.Core.Extensions;
using CrossStitch.Core.Interfaces;
using CrossStitch.Core.Models;
using CrossStitch.Core.Models.SearchCriteria;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace CrossStitch.Core.ViewModels
{
    [Title("Manage Orders")]
    public class ManageOrdersViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogService;
        private readonly OrderRepository _orderRepository;

        public ManageOrdersViewModel(IDialogService dialogService, OrderRepository orderRepository)
        {
            Criteria = new OrderSearchCriteria();

            _dialogService = dialogService;
            _orderRepository = orderRepository;

            NewOrderCommand = new RelayCommand(OnNewOrder);
            EditOrderCommand = new RelayCommand<Order>(OnEditOrder);
            DeleteOrderCommand = new RelayCommand<Order>(OnDeleteOrder);
            RefreshOrdersCommand = new RelayCommand(PopulateOrders);
        }

        public OrderSearchCriteria Criteria { get; set; }
        public ICommand DeleteOrderCommand { get; }
        public ICommand EditOrderCommand { get; }
        public ICommand NewOrderCommand { get; }
        public ObservableCollection<Order> Orders { get; set; } = new ObservableCollection<Order>();
        public ICommand RefreshOrdersCommand { get; }
        public Order SelectedItem { get; set; }

        public override void Initialise(object param)
        {
            Criteria = new OrderSearchCriteria();
            PopulateOrders();
        }

        public override void OnBack() => Initialise(null);

        private void OnDeleteOrder(Order order)
        {
            if (!_dialogService.ShowQuestion($"Are you sure you want to delete order \"{order.Threads}\"", "Confirm Order Deletion"))
                return;

            _orderRepository.Remove(order);
        }

        private void OnEditOrder(Order order)
        {
            Navigate<EditOrderViewModel>(order);
        }

        private void OnNewOrder()
        {
            Navigate<EditOrderViewModel>(new Order());
        }

        private void PopulateOrders()
        {
            var repoPatterns = _orderRepository.GetAll();
            Orders = Criteria.StatusFilter == OrderState.None ? repoPatterns.AsObservable() : repoPatterns.Where(r => r.Status == Criteria.StatusFilter).AsObservable();
        }
    }
}