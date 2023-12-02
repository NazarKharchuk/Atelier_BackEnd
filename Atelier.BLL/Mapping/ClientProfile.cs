using Atelier.BLL.DTO;
using Atelier.DAL.Entities;
using AutoMapper;

namespace Atelier.BLL.Mapping
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<Client, ClientDTO>().ReverseMap();
        }
    }
}
