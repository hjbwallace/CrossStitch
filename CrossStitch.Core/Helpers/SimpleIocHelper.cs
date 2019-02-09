using CrossStitch.Core.Extensions;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrossStitch.Core.Helpers
{
    public static class SimpleIocHelper
    {
        public static void ConfigureImplementations<T>(this IEnumerable<Type> types)
        {
            var typesToRegister = types.GetConcreteImplementations<T>();

            foreach (var typeToRegister in typesToRegister)
                RegisterConcreteType(typeToRegister);
        }

        public static void RegisterConcreteType(this Type implementationType)
        {
            var methodInfo =
                SimpleIoc.Default.GetType().GetMethods()
                         .Where(m => m.Name == "Register")
                         .Where(m => m.IsGenericMethod)
                         .Where(m => m.GetGenericArguments().Length == 1)
                         .Where(m => m.GetParameters().Length == 0)
                         .Single();

            methodInfo = methodInfo.MakeGenericMethod(implementationType);
            methodInfo.Invoke(SimpleIoc.Default, null);
        }
    }
}