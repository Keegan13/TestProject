using Infrastructure.Entities;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Linq
{
    public static partial class QueryableExtensions
    {
        public static IQueryable<TEntity> Search<TEntity>(this IQueryable<TEntity> source, string keywords)
        {
            if (string.IsNullOrEmpty(keywords)) return source;

            var properties = typeof(TEntity).GetProperties()
                .Where(x => x.PropertyType == typeof(string));

            Expression whereExpression = null;

            var parameter = Expression.Parameter(typeof(TEntity), "x");

            foreach (var prop in properties)
            {
                var propertyAccess = Expression.MakeMemberAccess(parameter, prop);
                var propertyExpression = Expression.Lambda(propertyAccess, parameter);


                //Create expression to represent x.[property] != null
                //var isNotNullExpression = Expression.NotEqual(propertyExpression, Expression.Constant(null));

                //Create expression to represent x.[property].Contains(searchTerm)
                var searchTermExpression = Expression.Constant(keywords);

                var checkContainsExpression = Expression.Call(propertyExpression.Body, typeof(string).GetMethod("Contains", new[] { typeof(string) }), searchTermExpression);

                //Join not null and contains expressions
                //var notNullAndContainsExpression = Expression.AndAlso(isNotNullExpression, checkContainsExpression);
                if (whereExpression == null)
                {
                    whereExpression = checkContainsExpression;
                }
                else
                    whereExpression = Expression.Or(whereExpression, checkContainsExpression);
            }

            var methodCallExpression = Expression.Call(typeof(Queryable),
                                                       "Where",
                                                       new Type[] { source.ElementType },
                                                       source.Expression,
                                                       Expression.Lambda<Func<TEntity, bool>>(whereExpression, parameter));

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

        public static IQueryable<TEntity> ApplyOrderModel<TEntity>(this IQueryable<TEntity> source, OrderModel model)
        {
            if (model == null) return source;

            var query = source;

            if (!string.IsNullOrEmpty(model.SortColumn))
            {
                query = model.isAscendingOrder ? query.OrderBy(model.SortColumn) : query.OrderByDescending(model.SortColumn);
            }

            if (model.Skip > 0)
                query = query.Skip(model.Skip);

            if (model.Take > 0)
                query = query.Take(model.Take);

            return query;
        }
    }
}