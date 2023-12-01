namespace Atelier.DAL.Entities
{
    public class WorksType
    {
        public int WorksTypeId { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }

        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
