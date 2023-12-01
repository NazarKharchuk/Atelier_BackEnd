using Atelier.DAL.Context;
using Atelier.DAL.Entities;
using Atelier.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Atelier.DAL.Repositories
{
    public class OrderRepository : IOrderRepository<Order>
    {
        private readonly AtelierContext db;

        public OrderRepository(AtelierContext context)
        {
            db = context;
        }

        public IQueryable<Order> GetAll()
        {
            return db.Orders;
        }

        public async Task<Order> Get(int id)
        {
            return await db.Orders.FindAsync(id);
        }

        public async Task Create(Order item)
        {
            await db.Orders.AddAsync(item);
        }

        public void Update(Order item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public List<Order> Find(Func<Order, Boolean> predicate)
        {
            return db.Orders.Where(predicate).ToList();
        }

        public async Task Delete(int id)
        {
            Order item = await db.Orders.FindAsync(id);
            if (item != null)
                db.Orders.Remove(item);
        }

        public void AddMaterial(Order order, OrderMaterial order_material)
        {
            order.OrdersMaterials.Add(order_material);
        }

        public List<OrderMaterial> GetAllMaterials(int order_id)
        {
            Order order = db.Orders.Include(n => n.OrdersMaterials).Where(t => t.OrderId == order_id).FirstOrDefault();

            List<OrderMaterial> materials = order.OrdersMaterials;

            return materials;
        }

        public void DeleteMaterial(int order_id, int material_id)
        {
            Order item = db.Orders.Include(x => x.OrdersMaterials).Where(x => x.OrderId == order_id).FirstOrDefault();
            if (item != null)
            {
                foreach (var i in item.Materials)
                {
                    if (i.MaterialId == material_id) item.Materials.Remove(i);
                    break;
                }
            }
        }

        public void UpdateMaterial(int order_id, OrderMaterial order_material)
        {
            Order item = db.Orders.Include(x => x.OrdersMaterials).Where(x => x.OrderId == order_id).FirstOrDefault();
            if (item != null)
            {
                foreach (var i in item.OrdersMaterials)
                {
                    if (i.MaterialId == order_material.MaterialId)
                    {
                        i.Count = order_material.Count;
                    }
                }
            }
        }

        public decimal GetMaterialCount(int order_id, int material_id)
        {
            decimal c = -1;
            Order item = db.Orders.Include(x => x.OrdersMaterials).Where(x => x.OrderId == order_id).FirstOrDefault();
            if (item != null)
            {
                foreach (var i in item.OrdersMaterials)
                {
                    if (i.MaterialId == material_id)
                    {
                        c = i.Count;
                    }
                }
            }
            return c;
        }

        public List<OrderResponse> GetOrdersWithDetails()
        {
            var orders = db.Orders.Include(x => x.Client).Include(x => x.Employee).Include(x => x.WorksType).Include(x => x.OrdersMaterials).ThenInclude(x => x.Material);

            Decimal price;
            List<OrderResponse> orderResponse = new List<OrderResponse>();
            foreach (var o in orders)
            {
                var or = new OrderResponse();
                price = o.WorksType.Cost;
                or.OrderId = o.OrderId;
                or.ReceivingDate = o.ReceivingDate;
                or.IssueDate = o.IssueDate;
                or.Status = o.Status;
                or.WorkTypeId = o.WorkTypeId;
                or.WorkTypeName = o.WorksType.Name;
                or.WorkTypeCost = o.WorksType.Cost;
                or.EmployeeId = o.EmployeeId;
                if (o.EmployeeId != null)
                {
                    or.EmployeeFirstName = o.Employee.FirstName;
                    or.EmployeeLastName = o.Employee.LastName;
                }
                or.ClientId = o.ClientId;
                or.ClientFirstName = o.Client.FirstName;
                or.ClientLastName = o.Client.LastName;
                or.Materials = new List<MaterialResponse>();
                foreach (var m in o.OrdersMaterials)
                {
                    var mr = new MaterialResponse();
                    price += m.Material.Cost * m.Count;
                    mr.MaterialId = m.MaterialId;
                    mr.Name = m.Material.Name;
                    mr.Cost = m.Material.Cost;
                    mr.Count = m.Count;
                    or.Materials.Add(mr);
                }
                or.Price = price;
                orderResponse.Add(or);
            }

            return orderResponse;
        }

        public async Task<decimal> CalculatePrice(int id)
        {
            var order = await db.Orders.Include(x => x.WorksType).Include(x => x.OrdersMaterials).ThenInclude(x => x.Material).Where(x => x.OrderId == id).FirstOrDefaultAsync();

            decimal price = order.WorksType.Cost;
            foreach (var om in order.OrdersMaterials)
            {
                price += om.Count * om.Material.Cost;
            }

            return price;
        }
    }
}
