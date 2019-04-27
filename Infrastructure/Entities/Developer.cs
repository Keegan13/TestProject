namespace Infrastructure.Entities
{
    using System.Collections.Generic;

    public class Developer
    {
        public Developer()
        {
            this.ProjectAssignments = new List<ProjectAssignment>();
        }

        public int Id { get; set; }

        public string FullName { get; set; }

        public string Nickname { get; set; }

        public ICollection<ProjectAssignment> ProjectAssignments { get; set; }
    }
}
