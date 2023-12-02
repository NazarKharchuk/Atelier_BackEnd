using Atelier.BLL.DTO;
using Atelier.PL.Models;
using AutoMapper;

namespace Atelier.PL.Mapping.Employee
{
    public class EmployeeModelProfile : Profile
    {
        public EmployeeModelProfile()
        {
            CreateMap<EmployeeModel, EmployeeDTO>().ReverseMap();
        }
    }
}
