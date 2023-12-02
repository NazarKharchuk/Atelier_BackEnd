using Atelier.BLL.DTO;
using Atelier.PL.Models;
using AutoMapper;

namespace Atelier.PL.Mapping.Request
{
    public class FilteredListRequestModelPrifile : Profile
    {
        public FilteredListRequestModelPrifile()
        {
            CreateMap<FilteredListRequestModel, FilteredListRequestDTO>().ReverseMap();
        }
    }
}
