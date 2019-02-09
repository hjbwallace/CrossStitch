using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CrossStitch.Core.Extensions
{
    public static class ReflectionExtensions
    {
        public static Type[] GetConcreteImplementations<T>(this IEnumerable<Type> types)
        {
            return types
                .Where(x => typeof(T).IsAssignableFrom(x))
                .Where(x => !x.GetTypeInfo().IsAbstract)
                .Where(x => !x.GetTypeInfo().IsInterface)
                .ToArray();
        }

        public static Type[] GetConcreteImplementations<T>()
        {
            return Assembly.GetCallingAssembly().GetTypes().GetConcreteImplementations<T>();
        }

        public static Type[] GetImplementations<T>()
        {
            return typeof(T).Assembly.GetLoadableTypes().GetConcreteImplementations<T>();
        }

        public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }

        public static MethodInfo GetGenericMethod<T>(string methodName, int numberOfArguments = 1)
        {
            return typeof(T).GetGenericMethod(methodName, numberOfArguments);
        }

        public static MethodInfo GetGenericMethod(this Type type, string methodName, int numberOfArguments = 1)
        {
            return type
                .GetMethods()
                .Where(m => m.Name == methodName)
                .Where(m => m.IsGenericMethod)
                .Where(m => m.GetGenericArguments().Length == numberOfArguments)
                .Where(m => m.GetParameters().Length == 0)
                .Single();
        }
    }
}