namespace Atelier.PL.Models
{
    public class FilteredListResponseModel<T>
    {
        public IEnumerable<T> List { get; set; }
        public int TotalCount { get; set; }
    }
}
