namespace Host.Models
{
    public class FilterModel
    {
        public string SortColumn { get; set; }
        public OrderDirection Order { get; set; }
        public int? Page { get; set; }
        public string Keywords { get; set; }
    }
}
