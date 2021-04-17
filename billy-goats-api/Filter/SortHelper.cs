using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BillyGoats.Api.Filter
{
    public static class SortHelper
    {
        private static readonly MethodInfo GroupByMethod =
            typeof(Queryable).GetMethods()
                .Where(method => method.Name == "GroupBy")
                .Where(method => method.GetParameters().Length == 2)
                .Single();

        private static readonly MethodInfo OrderByMethod =
            typeof(Queryable).GetMethods()
                .Where(method => method.Name == "OrderBy")
                .Where(method => method.GetParameters().Length == 2)
                .Single();

        private static readonly MethodInfo OrderByDescendingMethod =
            typeof(Queryable).GetMethods()
                .Where(method => method.Name == "OrderByDescending")
                .Where(method => method.GetParameters().Length == 2)
                .Single();

        public static IQueryable<T> OrderByProperty<T>(this IQueryable<T> source, string propertyName) where T : class
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return source;
            }

            var parameter = Expression.Parameter(typeof(T), propertyName);

            var orderByProperty = Expression.Property(parameter, propertyName);

            var lambda = Expression.Lambda(orderByProperty, new[] { parameter });
            Console.WriteLine(lambda);
            var genericMethod = OrderByMethod.MakeGenericMethod(new[] { typeof(T), orderByProperty.Type });
            var ret = genericMethod.Invoke(null, new object[] { source, lambda });
            return (IQueryable<T>)ret;
        }

        public static IQueryable<T> OrderByDescProperty<T>(this IQueryable<T> source, string propertyName) where T : class
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return source;
            }

            var parameter = Expression.Parameter(typeof(T), propertyName);

            var orderByProperty = Expression.Property(parameter, propertyName);

            var lambda = Expression.Lambda(orderByProperty, new[] { parameter });
            Console.WriteLine(lambda);
            MethodInfo genericMethod = OrderByDescendingMethod.MakeGenericMethod(new[] { typeof(T), orderByProperty.Type });
            object ret = genericMethod.Invoke(null, new object[] { source, lambda });
            return (IQueryable<T>)ret;
        }

        public static IQueryable<T> GroupByProperty<T>(this IQueryable<T> source, string propertyName) where T : class
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return source;
            }

            var parameter = Expression.Parameter(typeof(T), propertyName);

            var groupByProperty = Expression.Property(parameter, propertyName);

            var lambda = Expression.Lambda(groupByProperty, new[] { parameter });
            Console.WriteLine(lambda);
            MethodInfo genericMethod = GroupByMethod.MakeGenericMethod(new[] { typeof(T), groupByProperty.Type });
            object ret = genericMethod.Invoke(null, new object[] { source, lambda });
            return (IQueryable<T>)ret;
        }
    }
}
