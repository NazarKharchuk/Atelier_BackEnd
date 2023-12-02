using Atelier.BLL.DTO;
using Atelier.PL.Models;
using AutoMapper;

namespace Atelier.PL.Mapping.Request
{
    public class AuthorizationResponseModelProfile : Profile
    {
        public AuthorizationResponseModelProfile()
        {
            CreateMap<AuthorizationResponseModel, AuthorizationResponseDTO>().ReverseMap();
        }
    }
}
