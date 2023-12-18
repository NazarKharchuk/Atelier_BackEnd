using Atelier.BLL.DTO;
using Atelier.BLL.Interfaces;
using Atelier.PL.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Atelier.PL.Controllers
{
    [Authorize]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService<EmployeeDTO> employeeService;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeService<EmployeeDTO> serv, IMapper mapper)
        {
            employeeService = serv;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [Route("api/employees")]
        [HttpGet]
        public IActionResult GetEmployees([FromQuery] FilteredListRequestModel filter)
        {
            var res = employeeService.GetEmployees(_mapper.Map<FilteredListRequestDTO>(filter));
            return new ObjectResult(new ResponseModel<FilteredListResponseModel<EmployeeModel>>()
            {
                Data = new FilteredListResponseModel<EmployeeModel>() { List = _mapper.Map<List<EmployeeModel>>(res.Item1), TotalCount = res.Item2 },
                Seccessfully = true
            });
        }

        [Authorize(Roles = "Admin")]
        [Route("api/employees/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var item = await employeeService.Get(id);
                await employeeService.Delete(id);
                return new ObjectResult(new ResponseModel<EmployeeModel>()
                {
                    Seccessfully = true
                });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new ResponseModel<EmployeeModel>() { Seccessfully = false, Code = 400, Message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [Route("api/employees")]
        [HttpPost]
        public async Task<IActionResult> PostEmployee([FromBody] EmployeeRegisterModel item)
        {
            try
            {
                await employeeService.Create(_mapper.Map<EmployeeDTO>(item), new UserDTO() { Password = item.Password, Login = item.Login, Role = DAL.Enums.Role.User });

                return new ObjectResult(new ResponseModel<EmployeeModel>()
                {
                    Seccessfully = true
                });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new ResponseModel<EmployeeModel>() { Seccessfully = false, Code = 400, Message = ex.Message });
            }
        }

        [Route("api/employees/firstNames")]
        [HttpGet]
        public IActionResult GetFirstNames()
        {
            return new ObjectResult(new ResponseModel<List<string>>() { Seccessfully = true, Data = employeeService.GetFirstNames() });
        }

        [Route("api/employees/selectData")]
        [HttpGet]
        public IActionResult GetDataForSelect()
        {
            return new ObjectResult(new ResponseModel<List<ResponseIdAndStringModel>>() { Seccessfully = true, Data = _mapper.Map<List<ResponseIdAndStringModel>>(employeeService.GetDataForSelect()) });
        }
    }
}
