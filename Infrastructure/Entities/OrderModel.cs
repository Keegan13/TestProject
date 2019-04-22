using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Entities
{
    public class OrderModel
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public string SortColumn { get; set; }
        public bool isAscendingOrder { get; set; }
    }
}
