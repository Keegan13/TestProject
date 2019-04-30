using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.Models
{
    public class ProjectFilterModel
    {
        public ProjectSort? Sort { get; set; }

        public OrderDirection? Order { get; set; }

        public int? Skip { get; set; }

        public int? Take { get; set; }

        public string Keywords { get; set; }

        public ProjectSet? Set { get; set; }

        public string DeveloperContextUrl { get; set; }
    }
}
