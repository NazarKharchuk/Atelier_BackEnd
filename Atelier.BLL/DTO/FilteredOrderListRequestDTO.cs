namespace Atelier.BLL.DTO
{
    public class FilteredOrderListRequestDTO
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int? Status { get; set; }
        public string EmployeeFirstName { get; set; }
        public string WorksTypeName { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public string Sort { get; set; }
    }
}
