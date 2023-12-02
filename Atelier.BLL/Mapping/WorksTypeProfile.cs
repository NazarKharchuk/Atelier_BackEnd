using Atelier.BLL.DTO;
using Atelier.DAL.Entities;
using AutoMapper;

namespace Atelier.BLL.Mapping
{
    public class WorksTypeProfile : Profile
    {
        public WorksTypeProfile()
        {
            CreateMap<WorksType, WorksTypeDTO>().ReverseMap();
        }
    }
}
