namespace Infrastructure.Entities
{
    using System;
    using System.Collections.Generic;

    public class Project
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public ProjectStatus Status { get; set; }

        public IEnumerable<ProjectDeveloper> ProjectDevelopers { get; set; }
    }
}
