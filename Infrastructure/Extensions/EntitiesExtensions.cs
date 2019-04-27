namespace Infrastructure
{
    using Infrastructure.Entities;
    using System.Linq;
    using System.Collections.Generic;

    public static partial class EntitiesExtensions
    {
        public static IEnumerable<Developer> Developers(this Project project) => project.ProjectAssignments.Select(x => x.Developer);

        public static IEnumerable<Project> Projects(this Developer developer) => developer.ProjectAssignments.Select(x => x.Project);
    }
}
