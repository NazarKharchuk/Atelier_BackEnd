using Atelier.BLL.DTO;
using Atelier.DAL.Entities;
using AutoMapper;

namespace Atelier.BLL.Mapping
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDTO>().ReverseMap();
        }
    }
}
