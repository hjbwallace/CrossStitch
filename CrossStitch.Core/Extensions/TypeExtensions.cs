using CrossStitch.Core.Helpers;
using System;

namespace CrossStitch.Core.Extensions
{
    public static class TypeExtensions
    {
        public static object GetDefault(this Type type)
        {
            return type.IsValueType
                ? Activator.CreateInstance(type)
                : null;
        }

        public static bool IsDefault<T>(this T data)
        {
            var key = KeyHelper.GetKeyProperty<T>();

            var keyValue = key.GetValue(data);
            var defaultValue = keyValue.GetType().GetDefault();

            return keyValue.Equals(defaultValue);
        }

        public static bool IsNullable(this Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }
    }
}