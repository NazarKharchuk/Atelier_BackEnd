using Atelier.BLL.DTO;
using Atelier.PL.Models;
using AutoMapper;

namespace Atelier.PL.Mapping.Request
{
    public class FilteredMaterialListRequestModelPrifile : Profile
    {
        public FilteredMaterialListRequestModelPrifile()
        {
            CreateMap<FilteredMaterialListRequestModel, FilteredMaterialListRequestDTO>().ReverseMap();
        }
    }
}
