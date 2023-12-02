using Atelier.BLL.DTO;
using Microsoft.Extensions.Configuration;

namespace Atelier.BLL.Interfaces
{
    public interface IUserService<UserDTO> : IService<UserDTO>
    {
        UserDTO GetByLogin(string login);
        Task Create(UserDTO item);
        Task<AuthorizationResponseDTO> Login(UserDTO model, IConfiguration _config);
        //UserDTO Authenticate(UserDTO userLogin);
        //string Generate(UserDTO user);
    }
}
