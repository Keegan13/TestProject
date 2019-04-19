using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.Models
{
    public class CollectionResult<T>
    {
        public T[] Values { get; set; }
        public int TotalCount { get; set; }
    }
}
