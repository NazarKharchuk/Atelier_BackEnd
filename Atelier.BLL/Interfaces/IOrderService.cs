using Atelier.BLL.DTO;
using Atelier.DAL.Entities;
using Atelier.DAL.Enums;

namespace Atelier.BLL.Interfaces
{
    public interface IOrderService<OrderDTO> : IService<OrderDTO>
    {
        List<OrderDTO> GetAll();
        Task<OrderDTO> Get(int id);
        Task CreateOrderWithMaterial(OrderDTO order, List<OrderMaterialDTO> om_list);
        Task Create(OrderDTO item);
        Task Update(OrderDTO item);
        Task AddMaterial(/*OrderDTO order*/ int OrderId, OrderMaterialDTO order_material);
        Task<List<OrderMaterialDTO>> GetAllMaterials(int order_id);
        Task DeleteMaterial(int order_id, int material_id);
        Task UpdateMaterial(int order_id, OrderMaterialDTO material);
        Tuple<List<OrderResponse>, int> GetOrders(FilteredOrderListRequestDTO filter);
        byte[] ExportOrders();
        Tuple<int, int> GetMaxMinYear();
        List<MonthStatisticResponseDTO> GetYearStatistic(int year);
        Task UpdateStatus(int order_id, Status newStatus);
    }
}
