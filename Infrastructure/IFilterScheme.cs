using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure
{
    public interface IFilterScheme<TEntity> where TEntity:class
    {
        IQueryable<TEntity> ApplyFilter(IQueryable<TEntity> source);
    }
}
