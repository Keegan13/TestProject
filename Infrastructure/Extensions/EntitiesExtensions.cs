namespace Infrastructure
{
    using Infrastructure.Entities;
    using System.Linq;
    using System.Collections.Generic;

    public static partial class EntitiesExtensions
    {
        public static IEnumerable<Developer> AssignedDevelopers(this Project project) => project.ProjectDevelopers.Select(x => x.Developer);
        public static IEnumerable<Project> Projects(this Developer developer) => developer.ProjectDevelopers.Select(x => x.Project);
    }
}
