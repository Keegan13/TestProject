using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.Models
{
    //TODO rename: PageResult, Page  etc
    public class PaginationCollection<T>
    {
        public T[] Values { get; set; }
        public int TotalCount { get; set; }
    }
}
