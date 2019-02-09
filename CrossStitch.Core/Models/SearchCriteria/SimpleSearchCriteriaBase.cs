using CrossStitch.Core.Extensions;
using CrossStitch.Core.Helpers;
using CrossStitch.Core.Interfaces;
using System;
using System.Linq;

namespace CrossStitch.Core.Models.SearchCriteria
{
    public abstract class SimpleSearchCriteriaBase<T> : NotifyPropertyChanged, ISearchCriteria<T>
    {
        public virtual Func<T, bool> Apply => x => ApplySearchable(x);

        public virtual bool IsValid => true;
        public string SearchString { get; set; } = string.Empty;

        protected bool ApplySearchable(T entity)
        {
            var properties = SearchableHelper.GetSearchableProperties<T>();
            return properties.Any(p => p.GetValue(entity).ToString().Contains(SearchString, StringComparison.OrdinalIgnoreCase));
        }
    }
}