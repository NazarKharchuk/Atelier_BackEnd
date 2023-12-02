using Atelier.BLL.DTO;
using Atelier.DAL.Entities;
using AutoMapper;

namespace Atelier.BLL.Mapping
{
    public class MaterialProfile : Profile
    {
        public MaterialProfile()
        {
            CreateMap<Material, MaterialDTO>().ReverseMap();
        }
    }
}
