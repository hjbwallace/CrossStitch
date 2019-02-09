using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CrossStitch.Core.Extensions
{
    public static class AttributeExtensions
    {
        public static T GetFirstAttribute<T>(this PropertyInfo[] propertyInfos) where T : Attribute
        {
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<T>();

                if (attribute != null)
                    return attribute;
            }

            return null;
        }

        public static KeyAttribute GetKey(this PropertyInfo[] propertyInfos)
        {
            return propertyInfos.GetFirstAttribute<KeyAttribute>();
        }
    }
}