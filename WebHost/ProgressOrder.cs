using Infrastructure;
using Infrastructure.Entities;
using System;
using System.Linq;

namespace Host
{
    public class OrderByProgressScheme : ISortScheme<Project>
    {
        public IOrderedQueryable<Project> AscendingOrder(IQueryable<Project> source)
        {
            var now = DateTime.Now;
            return source.OrderBy(x => (now - x.StartDate) / (x.EndDate - x.StartDate));
        }

        public IOrderedQueryable<Project> DescendingOrder(IQueryable<Project> source)
        {
            var now = DateTime.Now;
            return source.OrderByDescending(x => (now - x.StartDate) / (x.EndDate - x.StartDate));
        }
    }
}
