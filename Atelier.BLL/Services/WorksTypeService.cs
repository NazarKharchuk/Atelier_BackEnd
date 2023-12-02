using Atelier.BLL.DTO;
using Atelier.BLL.Infrastructure;
using Atelier.BLL.Interfaces;
using Atelier.DAL.Entities;
using Atelier.DAL.Interfaces;
using AutoMapper;

namespace Atelier.BLL.Services
{
    public class WorksTypeService : IWorksTypeService<WorksTypeDTO>
    {
        IUnitOfWork DataBase { get; set; }
        private readonly IMapper _mapper;

        public WorksTypeService(IUnitOfWork uow, IMapper mapper)
        {
            DataBase = uow;
            _mapper = mapper;
        }

        public List<WorksTypeDTO> GetAll()
        {
            return _mapper.Map<List<WorksTypeDTO>>(DataBase.WorksTypes.GetAll());
        }

        public Tuple<List<WorksTypeDTO>, int> GetWorksTypes(FilteredListRequestDTO filter)
        {
            IEnumerable<WorksType> worksTypes = DataBase.WorksTypes.GetAll();
            int count = worksTypes.Count();
            if (filter.PageNumber == null) filter.PageNumber = 1;
            if (filter.PageSize == null) { filter.PageSize = 10; };
            if (filter.PageSize > 20) { filter.PageSize = 20; };
            worksTypes = worksTypes.Skip((int)((filter.PageNumber - 1) * filter.PageSize)).Take((int)filter.PageSize);
            return Tuple.Create(_mapper.Map<List<WorksTypeDTO>>(worksTypes), count);
        }

        public async Task<WorksTypeDTO> GetById(int id)
        {
            var item = await DataBase.WorksTypes.Get(id);
            if (item == null)
                throw new Exception("Послугу з таким ідентифікатором не знайдено");
            return _mapper.Map<WorksTypeDTO>(item);
        }

        public async Task Create(WorksTypeDTO item)
        {
            if (item.Name == "")
                throw new ValidationException("Порожня назва послуги", "");
            if (item.Cost <= 0)
                throw new ValidationException("Не коректна вартість послуги", "");

            await DataBase.WorksTypes.Create(_mapper.Map<WorksType>(item));
            await DataBase.SaveAsync();
        }
        public async Task Update(WorksTypeDTO item)
        {
            if (item.Name == "")
                throw new ValidationException("Порожня назва послуги", "");
            if (item.Cost <= 0)
                throw new ValidationException("Не коректна вартість послуги", "");

            WorksType works_type = await DataBase.WorksTypes.Get(item.WorksTypeId);
            if (works_type == null)
                throw new Exception("Послугу не знайдено");
            works_type.Name = item.Name;
            works_type.Cost = item.Cost;
            DataBase.WorksTypes.Update(works_type);
            await DataBase.SaveAsync();
        }

        public List<string> GetNames()
        {
            return (DataBase.WorksTypes.GetAll().Select(x => x.Name).Distinct().ToList());
        }

        public List<ResponseIdAndStringDTO> GetDataForSelect()
        {
            var list = DataBase.WorksTypes.GetAll();
            var result = new List<ResponseIdAndStringDTO>();
            foreach (var l in list)
            {
                result.Add(new ResponseIdAndStringDTO() { Id = l.WorksTypeId, Str = "id: " + l.WorksTypeId + "; назва: " + l.Name + ";" });
            }
            return result;
        }

        public void Dispose()
        {
            DataBase.Dispose();
        }
    }
}
