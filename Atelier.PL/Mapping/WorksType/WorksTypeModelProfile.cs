using Atelier.BLL.DTO;
using Atelier.PL.Models;
using AutoMapper;

namespace Atelier.PL.Mapping.WorksType
{
    public class WorksTypeModelProfile : Profile
    {
        public WorksTypeModelProfile()
        {
            CreateMap<WorksTypeModel, WorksTypeDTO>().ReverseMap();
        }
    }
}
