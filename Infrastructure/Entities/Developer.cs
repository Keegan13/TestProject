namespace Infrastructure.Entities
{
    using System.Collections.Generic;

    public class Developer
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Nickname { get; set; }

        public IEnumerable<ProjectAssignment> ProjectAssignments { get; set; }
    }
}
