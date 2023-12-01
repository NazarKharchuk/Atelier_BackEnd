using Atelier.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atelier.DAL.Interfaces
{
    public interface IOrderRepository<Order> : IRepository<Order>
    {
        void AddMaterial(Order order, OrderMaterial order_material);
        List<OrderMaterial> GetAllMaterials(int order_id);
        void UpdateMaterial(int order_id, OrderMaterial order_material);
        void DeleteMaterial(int order_id, int material_id);
        decimal GetMaterialCount(int order_id, int material_id);
        List<OrderResponse> GetOrdersWithDetails();
    }
}
