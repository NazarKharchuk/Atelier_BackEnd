namespace Atelier.PL.Models
{
    public class FilteredMaterialListRequestModel
    {
        public string Name { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public string Sort { get; set; }
    }
}
