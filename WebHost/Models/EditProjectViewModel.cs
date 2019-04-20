using Infrastructure.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Host.Models
{
    public class EditProjectViewModel
    {
        public string Url { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        [Required]
        public DateTime? EndDate { get; set; }
        public ProjectStatus? Status { get; set; }
        public string Developer { get; internal set; }
    }
}
