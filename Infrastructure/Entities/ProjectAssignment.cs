namespace Infrastructure.Entities
{
    //bind entity for many to many
     public class ProjectAssignment
    {
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public int DeveloperId { get; set; }
        public Developer Developer { get; set; }
    }
}
