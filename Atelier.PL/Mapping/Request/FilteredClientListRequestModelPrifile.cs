using Atelier.BLL.DTO;
using Atelier.PL.Models;
using AutoMapper;

namespace Atelier.PL.Mapping.Request
{
    public class FilteredClientListRequestModelPrifile : Profile
    {
        public FilteredClientListRequestModelPrifile()
        {
            CreateMap<FilteredClientListRequestModel, FilteredClientListRequestDTO>().ReverseMap();
        }
    }
}
