namespace Host.Models
{
    public class FilterModel
    {
        public string Type { get; set; }

        public string Sort { get; set; }

        public OrderDirection? Order { get; set; }

        public int? Skip { get; set; }

        public int? Take { get; set; }

        public string Keywords { get; set; }

        public string Set { get; set; }

        public string Context { get; set; }
    }
}
