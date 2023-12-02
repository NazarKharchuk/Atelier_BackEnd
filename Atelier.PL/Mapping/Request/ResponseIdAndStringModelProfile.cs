using Atelier.BLL.DTO;
using Atelier.PL.Models;
using AutoMapper;

namespace Atelier.PL.Mapping.Request
{
    public class ResponseIdAndStringModelProfile : Profile
    {
        public ResponseIdAndStringModelProfile()
        {
            CreateMap<ResponseIdAndStringModel, ResponseIdAndStringDTO>().ReverseMap();
        }
    }
}
