using Atelier.BLL.DTO;

namespace Atelier.BLL.Interfaces
{
    public interface IWorksTypeService<WorksTypeDTO> : IService<WorksTypeDTO>
    {
        List<WorksTypeDTO> GetAll();
        Task<WorksTypeDTO> GetById(int id);
        Task Create(WorksTypeDTO item);
        Task Update(WorksTypeDTO item);
        Tuple<List<WorksTypeDTO>, int> GetWorksTypes(FilteredListRequestDTO filter);
        List<string> GetNames();
        List<ResponseIdAndStringDTO> GetDataForSelect();
    }
}
