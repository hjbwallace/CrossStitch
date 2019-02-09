using System;

namespace CrossStitch.Core.Models.SearchCriteria
{
    public class InventorySearchCriteria : SimpleSearchCriteriaBase<ThreadReference>
    {
        public override Func<ThreadReference, bool> Apply => tr => base.Apply(tr) && (ShowAllThreads ? true : tr.Owned);

        public bool ShowAllThreads { get; set; } = true;
    }
}