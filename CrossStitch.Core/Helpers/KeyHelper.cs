using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace CrossStitch.Core.Helpers
{
    public static class KeyHelper
    {
        private static ConcurrentDictionary<Type, PropertyInfo> _keyProperties = new ConcurrentDictionary<Type, PropertyInfo>();

        public static PropertyInfo GetKeyProperty<T>()
        {
            var type = typeof(T);
            var keyProperty = _keyProperties.GetOrAdd(type, GetPropertyInfo);

            if (keyProperty == null)
                throw new InvalidOperationException($"{type.FullName} does not have a KeyAttribute field");

            return keyProperty;
        }

        public static T GetKeyValue<T>(object source)
        {
            var keyProperty = GetKeyProperty<T>();
            return (T)keyProperty.GetValue(source);
        }

        private static PropertyInfo GetPropertyInfo(Type type)
        {
            return type.GetProperties().SingleOrDefault(x => x.GetCustomAttribute<KeyAttribute>() != null);
        }
    }
}