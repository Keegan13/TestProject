﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Host.Models
{
    public class EditDeveloperViewModel
    {
        public string Url { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Nickname { get; set; }
        public IEnumerable<string> Skills { get; set; }
        public string Project { get; set; }
    }
}
