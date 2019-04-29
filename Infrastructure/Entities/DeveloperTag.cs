using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Entities
{
    public class DeveloperTag
    {
        public int DeveloperId { get; set; }
        public int TagId { get; set; }
        public Developer Developer { get; set; }
        public Tag Tag { get; set; }
    }
}
