using Atelier.BLL.DTO;
using Atelier.PL.Models;
using AutoMapper;

namespace Atelier.PL.Mapping.Material
{
    public class MaterialModelProfile : Profile
    {
        public MaterialModelProfile()
        {
            CreateMap<MaterialModel, MaterialDTO>().ReverseMap();
        }
    }
}
