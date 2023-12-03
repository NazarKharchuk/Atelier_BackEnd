using Atelier.DAL.Enums;

namespace Atelier.BLL.DTO
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public DateTime ReceivingDate { get; set; }
        public DateTime IssueDate { get; set; }
        public Status Status { get; set; }
        public int WorkTypeId { get; set; }
        public int? EmployeeId { get; set; }
        public int ClientId { get; set; }
    }
}
