using Atelier.BLL.DTO;
using Atelier.PL.Models;
using AutoMapper;

namespace Atelier.PL.Mapping.Request
{
    public class FilteredOrderListRequestModelPrifile : Profile
    {
        public FilteredOrderListRequestModelPrifile()
        {
            CreateMap<FilteredOrderListRequestModel, FilteredOrderListRequestDTO>().ReverseMap();
        }
    }
}
