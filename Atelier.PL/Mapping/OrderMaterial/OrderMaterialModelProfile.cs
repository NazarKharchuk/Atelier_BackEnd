using Atelier.BLL.DTO;
using Atelier.PL.Models;
using AutoMapper;

namespace Atelier.PL.Mapping.OrderMaterial
{
    public class OrderMaterialModelProfile : Profile
    {
        public OrderMaterialModelProfile()
        {
            CreateMap<OrderMaterialModel, OrderMaterialDTO>().ReverseMap();
        }
    }
}
