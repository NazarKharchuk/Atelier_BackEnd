using Atelier.BLL.DTO;
using Atelier.PL.Models;
using AutoMapper;

namespace Atelier.PL.Mapping.Request
{
    public class MonthStatisticResponseModelProfile : Profile
    {
        public MonthStatisticResponseModelProfile()
        {
            CreateMap<MonthStatisticResponseModel, MonthStatisticResponseDTO>().ReverseMap();
        }
    }
}
