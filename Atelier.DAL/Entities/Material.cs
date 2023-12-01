namespace Atelier.DAL.Entities
{
    public class Material
    {
        public int MaterialId { get; set; }
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public decimal Reserve { get; set; }
        public decimal Cost { get; set; }

        public List<Order> Orders { get; set; } = new List<Order>();
        public List<OrderMaterial> OrdersMaterials { get; set; } = new List<OrderMaterial>();
    }
}
