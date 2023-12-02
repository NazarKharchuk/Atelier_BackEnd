namespace Atelier.BLL.DTO
{
    public class FilteredMaterialListRequestDTO
    {
        public string Name { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public string Sort { get; set; }
    }
}
