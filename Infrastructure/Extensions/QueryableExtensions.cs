using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Linq
{
    public static class QueryableExtensions
    {
        public static IQueryable<TEntity> Search<TEntity>(this IQueryable<TEntity> source, string keywords)
        {
            List<PropertyInfo> properties = new List<PropertyInfo>();
            foreach (var prop in typeof(TEntity).GetProperties(BindingFlags.Public))
            {
                if (prop.PropertyType == typeof(string))
                {
                    properties.Add(prop);
                }
            }

            Expression whereExpression = null;

            foreach (var prop in properties)
            {
                // The below represents the following lamda:
                // source.Where(x => x.[property] != null
                //                && x.[property].Contains(searchTerm))
                var parameter = Expression.Parameter(typeof(TEntity), "x");
                var propertyAccess = Expression.MakeMemberAccess(parameter, prop);
                var propertyExpression = Expression.Lambda(propertyAccess, parameter);


                //Create expression to represent x.[property] != null
                var isNotNullExpression = Expression.NotEqual(propertyExpression, Expression.Constant(null));

                //Create expression to represent x.[property].Contains(searchTerm)
                var searchTermExpression = Expression.Constant(keywords);

                var checkContainsExpression = Expression.Call(propertyExpression, typeof(string).GetMethod("Contains"), searchTermExpression);

                //Join not null and contains expressions
                var notNullAndContainsExpression = Expression.AndAlso(isNotNullExpression, checkContainsExpression);

                whereExpression = whereExpression==null?notNullAndContainsExpression:Expression.Or(whereExpression,notNullAndContainsExpression);
            }
            var methodCallExpression = Expression.Call(typeof(Queryable),
                                                       "Where",
                                                       new Type[] { source.ElementType },
                                                       source.Expression,
                                                       Expression.Lambda<Func<TEntity, bool>>(whereExpression));

            return source.Provider.CreateQuery<TEntity>(methodCallExpression);
        }

        public static IOrderedQueryable<TEntity> OrderByDescending<TEntity>(this IQueryable<TEntity> source, string orderByProperty)
        {
            return Order(source, orderByProperty, false);
        }

        public static IOrderedQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string orderByProperty)
        {
            return Order(source, orderByProperty, true);
        }
        private static IOrderedQueryable<TEntity> Order<TEntity>(IQueryable<TEntity> source, string orderByProperty, bool isAsc)
        {
            PropertyInfo property = null;
            foreach (var prop in typeof(TEntity).GetProperties())
            {
                if (string.Compare(prop.Name, orderByProperty, true) == 0)
                {
                    property = prop;
                    break;
                }
            }
            if (property != null)
            {
                var parameter = Expression.Parameter(typeof(TEntity), "x");
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExpression = Expression.Lambda(propertyAccess, parameter);
                var resultExpression = Expression.Call(typeof(Queryable), isAsc ? "OrderBy" : "OrderByDescending", new Type[] { typeof(TEntity), property.PropertyType }, source.Expression, Expression.Quote(orderByExpression));
                return (IOrderedQueryable<TEntity>)source.Provider.CreateQuery<TEntity>(resultExpression);
            }
            return (IOrderedQueryable<TEntity>)source;
        }
    }
}