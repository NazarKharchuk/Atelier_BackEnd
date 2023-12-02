using Atelier.BLL.DTO;

namespace Atelier.BLL.Interfaces
{
    public interface IClientService<ClientDTO> : IService<ClientDTO>
    {
        List<ClientDTO> GetAll();
        Task<ClientDTO> GetById(int id);
        Task Create(ClientDTO item);
        Task Update(ClientDTO item);
        Tuple<List<ClientDTO>, int> GetClients(FilteredClientListRequestDTO filter);
        List<string> GetFirstNames();
        List<string> GetLastNames();
        List<ResponseIdAndStringDTO> GetDataForSelect();
    }
}
