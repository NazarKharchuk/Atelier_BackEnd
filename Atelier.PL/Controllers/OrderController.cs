using Atelier.BLL.DTO;
using Atelier.BLL.Interfaces;
using Atelier.DAL.Entities;
using Atelier.DAL.Enums;
using Atelier.PL.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Atelier.PL.Controllers
{
    //[Authorize]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService<OrderDTO> orderService;
        private readonly IMapper _mapper;

        public OrderController(IOrderService<OrderDTO> serv, IMapper mapper)
        {
            orderService = serv;
            _mapper = mapper;
        }

        [Route("api/orders")]
        [HttpGet]
        public IActionResult GetOrders([FromQuery] FilteredOrderListRequestModel filter)
        {
            var res = orderService.GetOrders(_mapper.Map<FilteredOrderListRequestDTO>(filter));
            return new ObjectResult(new ResponseModel<FilteredListResponseModel<OrderResponse>>()
            {
                Data = new FilteredListResponseModel<OrderResponse>() { List = res.Item1, TotalCount = res.Item2 },
                Seccessfully = true,
            });
        }

        [Route("api/orders/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetOrder(int id)
        {
            try
            {
                var item = await orderService.Get(id);

                return new ObjectResult(new ResponseModel<OrderModel>()
                {
                    Seccessfully = true,
                    Data = _mapper.Map<OrderModel>(item)
                });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new ResponseModel<OrderModel>() { Seccessfully = false, Code = 404, Message = ex.Message });
            }

        }

        [Route("api/orders")]
        [HttpPost]
        public async Task<IActionResult> PostOrder([FromBody] OrderRequestModel res)
        {
            try
            {
                await orderService.CreateOrderWithMaterial(_mapper.Map<OrderDTO>(res.res_order), _mapper.Map<List<OrderMaterialDTO>>(res.res_materials));

                return new ObjectResult(new ResponseModel<OrderModel>()
                {
                    Seccessfully = true
                });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new ResponseModel<OrderModel>() { Seccessfully = false, Code = 400, Message = ex.Message });
            }
        }

        [Route("api/orders/{id}")]
        [HttpPut]
        public async Task<IActionResult> PutOrder(int id, [FromBody] OrderModel item)
        {
            if (id != item.OrderId)
            {
                return new ObjectResult(new ResponseModel<OrderModel>() { Seccessfully = false, Code = 400, Message = "Різні ідентифікатори" });
            }

            try
            {
                await orderService.Update(_mapper.Map<OrderDTO>(item));

                return new ObjectResult(new ResponseModel<OrderModel>() { Seccessfully = true });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new ResponseModel<OrderModel>() { Seccessfully = false, Code = 404, Message = ex.Message });
            }
        }

        [Route("api/orders/{id}/status")]
        [HttpPut]
        public async Task<IActionResult> PutOrderStatus(int id, [FromBody] Status newStatus)
        {
            try
            {
                await orderService.UpdateStatus(id, newStatus);

                return new ObjectResult(new ResponseModel<string>() { Seccessfully = true });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new ResponseModel<string>() { Seccessfully = false, Code = 404, Message = ex.Message });
            }
        }

        [Route("api/orders/{orderId}/materials")]
        [HttpGet]
        public async Task<IActionResult> GetOrderMaterials(int orderId)
        {
            var item = _mapper.Map<List<OrderMaterialModel>>(await orderService.GetAllMaterials(orderId));
            return new ObjectResult(new ResponseModel<List<OrderMaterialModel>>()
            {
                Seccessfully = true,
                Data = item
            });
        }

        [Route("api/orders/{orderId}/materials")]
        [HttpPost]
        public async Task<IActionResult> PostOrderMaterial(int orderId, [FromBody] OrderMaterialModel item)
        {
            try
            {
                await orderService.AddMaterial(orderId, _mapper.Map<OrderMaterialDTO>(item));

                return new ObjectResult(new ResponseModel<OrderMaterialDTO>()
                {
                    Seccessfully = true
                });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new ResponseModel<OrderMaterialDTO>() { Seccessfully = false, Code = 400, Message = ex.Message });
            }
        }

        [Route("api/orders/{orderId}/materials/{materialId}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteOrderMaterial(int orderId, int materialId)
        {
            try
            {
                await orderService.DeleteMaterial(orderId, materialId);

                return new ObjectResult(new ResponseModel<string>() { Seccessfully = true });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new ResponseModel<string>() { Seccessfully = false, Code = 400, Message = ex.Message });
            }
        }

        [Route("api/orders/{orderId}/materials/{materialId}")]
        [HttpPut]
        public async Task<IActionResult> PutOrderMaterial(int orderId, int materialId, [FromBody] OrderMaterialModel item)
        {
            if (materialId != item.MaterialId || orderId != item.OrderId)
            {
                return new ObjectResult(new ResponseModel<string>() { Seccessfully = false, Code = 400, Message = "Різні ідентифікатори" });
            }
            try
            {
                await orderService.UpdateMaterial(orderId, _mapper.Map<OrderMaterialDTO>(item));

                return new ObjectResult(new ResponseModel<string>() { Seccessfully = true });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new ResponseModel<string>() { Seccessfully = false, Code = 404, Message = ex.Message });
            }
        }

        [Route("api/orders/years")]
        [HttpGet]
        public IActionResult GetMaxMinYears()
        {
            var res = orderService.GetMaxMinYear();
            return new ObjectResult(new ResponseModel<MaxAndMinOrderYearResponse>()
            {
                Data = new MaxAndMinOrderYearResponse() { MaxYear = res.Item1, MinYear = res.Item2 },
                Seccessfully = true,
            });
        }

        [Route("api/orders/statisticalData/{year}")]
        [HttpGet]
        public IActionResult GetStatisticalData(int year)
        {
            var res = orderService.GetYearStatistic(year);
            return new ObjectResult(new ResponseModel<List<MonthStatisticResponseModel>>()
            {
                Data = _mapper.Map<List<MonthStatisticResponseModel>>(res),
                Seccessfully = true,
            });
        }

        [Route("api/orders/exportData")]
        [HttpGet]
        public IActionResult GetExportMaterials()
        {
            var bytes = orderService.ExportOrders();
            return File(bytes, "text/csv", "Orders.csv");
        }
    }
}
