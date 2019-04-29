using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<DeveloperTag> DeveloperTags { get; set; }
    }
}
