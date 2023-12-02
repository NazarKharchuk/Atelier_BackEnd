using Atelier.BLL.DTO;
using Atelier.PL.Models;
using AutoMapper;

namespace Atelier.PL.Mapping.User
{
    public class UserLoginModelProfile : Profile
    {
        public UserLoginModelProfile()
        {
            CreateMap<UserLoginModel, UserDTO>().ReverseMap();
        }
    }
}
