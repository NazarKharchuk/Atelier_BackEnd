using Atelier.DAL.Enums;

namespace Atelier.DAL.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime ReceivingDate { get; set; }
        public DateTime IssueDate { get; set; }
        public Status Status { get; set; }

        public int WorkTypeId { get; set; }
        public WorksType WorksType { get; set; }

        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }

        public List<Material> Materials { get; set; } = new List<Material>();
        public List<OrderMaterial> OrdersMaterials { get; set; } = new List<OrderMaterial>();
    }
}
