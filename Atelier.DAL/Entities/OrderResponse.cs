using Atelier.DAL.Enums;

namespace Atelier.DAL.Entities
{
    public class OrderResponse
    {
        public int OrderId { get; set; }
        public DateTime ReceivingDate { get; set; }
        public DateTime IssueDate { get; set; }
        public Status Status { get; set; }
        public int WorkTypeId { get; set; }
        public string WorkTypeName { get; set; }
        public decimal WorkTypeCost { get; set; }
        public int? EmployeeId { get; set; }
        public string EmployeeFirstName { get; set; }
        public string EmployeeLastName { get; set; }
        public int ClientId { get; set; }
        public string ClientFirstName { get; set; }
        public string ClientLastName { get; set; }
        public decimal Price { get; set; }
        public List<MaterialResponse> Materials { get; set; }
    }
}
