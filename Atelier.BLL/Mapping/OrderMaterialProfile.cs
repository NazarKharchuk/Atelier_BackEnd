using Atelier.BLL.DTO;
using Atelier.DAL.Entities;
using AutoMapper;

namespace Atelier.BLL.Mapping
{
    public class OrderMaterialProfile : Profile
    {
        public OrderMaterialProfile()
        {
            CreateMap<OrderMaterial, OrderMaterialDTO>().ReverseMap();
        }
    }
}
