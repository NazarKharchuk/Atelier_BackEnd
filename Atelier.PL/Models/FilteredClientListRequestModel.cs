namespace Atelier.PL.Models
{
    public class FilteredClientListRequestModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public string? Sort { get; set; }
    }
}
