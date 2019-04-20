using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Host.Models
{
    public class AssignModel
    {
        [Required]
        public string Project { get; set; }
        [Required]
        public string Developer { get; set; }
    }
}
