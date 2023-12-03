using Atelier.BLL.DTO;
using Atelier.PL.Models;
using AutoMapper;

namespace Atelier.PL.Mapping.OrderMaterial
{
    public class OrderRequestMaterialModelProfile : Profile
    {
        public OrderRequestMaterialModelProfile()
        {
            CreateMap<OrderRequestMaterialModel, OrderMaterialDTO>().ReverseMap();
        }
    }
}
