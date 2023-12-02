using Atelier.BLL.DTO;
using Atelier.PL.Models;
using AutoMapper;

namespace Atelier.PL.Mapping.Client
{
    public class ClientModelProfile : Profile
    {
        public ClientModelProfile()
        {
            CreateMap<ClientModel, ClientDTO>().ReverseMap();
        }
    }
}
