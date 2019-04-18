namespace Infrastructure.Entities
{
    //bind entity for many to many
     public class ProjectDeveloper
    {
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public int DeveloperId { get; set; }
        public Developer Developer { get; set; }
    }
}
