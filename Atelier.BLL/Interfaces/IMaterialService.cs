using Atelier.BLL.DTO;

namespace Atelier.BLL.Interfaces
{
    public interface IMaterialService<MaterialDTO> : IService<MaterialDTO>
    {
        List<MaterialDTO> GetAll();
        Task<MaterialDTO> Get(int id);
        Task Create(MaterialDTO item);
        Task Update(MaterialDTO item);
        Task<decimal> GetAvailableQuantity(int id);
        Task UpdateReserve(int id, decimal value);
        Task UpdateQuantity(int id, decimal value);
        Tuple<List<MaterialDTO>, int> GetMaterials(FilteredMaterialListRequestDTO filter);
        List<string> GetMaterialNames();
        List<ResponseIdAndStringDTO> GetDataForSelect();
        byte[] ExportMaterials();
    }
}
