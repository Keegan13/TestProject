namespace Host.Models
{
    public class ProjectsCollectionViewModel
    {
        public EditProjectViewModel[] Projects { get; set; } 
        public int PageCount { get; set; }
        public int Page { get; set; }
    }
}
