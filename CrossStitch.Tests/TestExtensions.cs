using CrossStitch.Core.Interfaces;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CrossStitch.Tests
{
    public static class TestExtensions
    {
        public static TEntity GetMostRecent<TEntity>(this IRepository<TEntity> repository)
        {
            return repository.GetAll().Last();
        }

        public static void SetProperty<TSource, TProperty>(this TSource source,
            Expression<Func<TSource, TProperty>> prop,
            TProperty value)
        {
            var propertyInfo = (PropertyInfo)((MemberExpression)prop.Body).Member;
            propertyInfo.SetValue(source, value);
        }
    }
}