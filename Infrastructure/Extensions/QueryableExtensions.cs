using System.Linq.Expressions;
using System.Reflection;

namespace System.Linq
{
    public static class QueryableExtensions
    {
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