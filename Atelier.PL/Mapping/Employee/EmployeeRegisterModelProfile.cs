using Atelier.BLL.DTO;
using Atelier.PL.Models;
using AutoMapper;

namespace Atelier.PL.Mapping.Employee
{
    public class EmployeeRegisterModelProfile : Profile
    {
        public EmployeeRegisterModelProfile()
        {
            CreateMap<EmployeeRegisterModel, EmployeeDTO>().ReverseMap();
        }
    }
}
