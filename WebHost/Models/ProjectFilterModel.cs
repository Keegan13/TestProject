namespace Host.Models
{
    public class FilterModel
    {
        public string SortColumn { get; set; }
        public OrderDirection SortOrder { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }
        public string Keywords { get; set; }
        public string SetName { get; set; }
        public string Context { get; set; }
    }
}
