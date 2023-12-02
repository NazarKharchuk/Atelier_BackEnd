using Atelier.BLL.DTO;

namespace Atelier.BLL.Interfaces
{
    public interface IEmployeeService<EmployeeDTO> : IService<EmployeeDTO>
    {
        List<EmployeeDTO> GetAll();
        Task<EmployeeDTO> Get(int id);
        Task Create(EmployeeDTO employee, UserDTO user);
        Task Delete(int id);
        Tuple<List<EmployeeDTO>, int> GetEmployees(FilteredListRequestDTO filter);
        List<string> GetFirstNames();
        List<ResponseIdAndStringDTO> GetDataForSelect();
    }
}
