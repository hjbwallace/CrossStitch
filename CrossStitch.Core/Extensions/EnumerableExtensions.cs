using CrossStitch.Core.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CrossStitch.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> ApplyCriteria<T>(this IEnumerable<T> source, ISearchCriteria<T> criteria)
        {
            return source.Where(criteria.Apply);
        }

        public static T[] AsArray<T>(this T source) => new[] { source };

        public static List<T> AsList<T>(this T source) => new List<T> { source };

        public static ObservableCollection<T> AsObservable<T>(this IEnumerable<T> source)
        {
            return new ObservableCollection<T>(source);
        }
    }
}