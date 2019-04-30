namespace Host.Models
{
    public class DeveloperFilterModel
    {
        public DeveloperSort? Sort { get; set; }

        public OrderDirection? Order { get; set; }

        public int? Skip { get; set; }

        public int? Take { get; set; }

        public string Keywords { get; set; }

        public DeveloperSet? Set { get; set; }

        public string ProjectContextUrl { get; set; }
    }
}
