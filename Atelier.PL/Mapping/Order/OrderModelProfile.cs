using Atelier.BLL.DTO;
using Atelier.PL.Models;
using AutoMapper;

namespace Atelier.PL.Mapping.Order
{
    public class OrderModelProfile : Profile
    {
        public OrderModelProfile()
        {
            CreateMap<OrderModel, OrderDTO>().ReverseMap();
        }
    }
}
