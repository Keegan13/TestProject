using Infrastructure.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Host.Models
{
    public class EditProjectViewModel
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public ProjectStatus Status { get; set; }

        public string Developer { get; set; }

        public string Url { get; set; }
    }
}
