using CrossStitch.Core.Attributes;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace CrossStitch.Core.Helpers
{
    public static class SearchableHelper
    {
        private static ConcurrentDictionary<Type, PropertyInfo[]> _searchableProperties = new ConcurrentDictionary<Type, PropertyInfo[]>();

        public static PropertyInfo[] GetSearchableProperties<T>()
        {
            var type = typeof(T);   
            var searchableProperties = _searchableProperties.GetOrAdd(type, GetPropertyInfo);

            if (searchableProperties?.Any() != true)
                throw new InvalidOperationException($"{type.FullName} does not have any SearchAttribute fields");

            return searchableProperties;
        }

        private static PropertyInfo[] GetPropertyInfo(Type type)
        {
            return type.GetProperties()
                       .Where(x => x.GetCustomAttribute<SearchAttribute>() != null)
                       .ToArray();
        }
    }
}