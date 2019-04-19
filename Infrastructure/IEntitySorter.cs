using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure
{
    public interface ISortScheme<TEntity>
    {
        IOrderedQueryable<TEntity> AscendingOrder(IQueryable<TEntity> source);
        IOrderedQueryable<TEntity> DescendingOrder(IQueryable<TEntity> source);
    }
}
