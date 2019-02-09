using CrossStitch.Core.Attributes;

namespace CrossStitch.Core.Models.SearchCriteria
{
    public class OrderSearchCriteria : NotifyPropertyChanged
    {
        [Lookup(typeof(OrderState))]
        [Ui("Status")]
        public OrderState StatusFilter { get; set; } = OrderState.None;
    }
}