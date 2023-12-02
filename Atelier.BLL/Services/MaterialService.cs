using Atelier.BLL.DTO;
using Atelier.BLL.Interfaces;
using Atelier.BLL.Infrastructure;
using Atelier.DAL.Entities;
using Atelier.DAL.Interfaces;
using AutoMapper;
using System.Text;

namespace Atelier.BLL.Services
{
    public class MaterialService : IMaterialService<MaterialDTO>
    {
        IUnitOfWork DataBase { get; set; }
        private readonly IMapper _mapper;

        public MaterialService(IUnitOfWork uow, IMapper mapper)
        {
            DataBase = uow;
            _mapper = mapper;
        }

        public List<MaterialDTO> GetAll()
        {
            return _mapper.Map<List<MaterialDTO>>(DataBase.Materials.GetAll());
        }

        public async Task<MaterialDTO> Get(int id)
        {
            var item = await DataBase.Materials.Get(id);
            if (item == null)
                throw new Exception("Матеріал не знайдено");

            return new MaterialDTO() { MaterialId = item.MaterialId, Name = item.Name, Cost = item.Cost, Quantity = item.Quantity, Reserve = item.Reserve };
        }

        public async Task Create(MaterialDTO item)
        {
            if (item.Name == "")
                throw new ValidationException("Порожня назва матеріалу", "");
            if (item.Quantity <= 0)
                throw new ValidationException("Не коректна загальна кількість матерііалу", "");
            if (item.Reserve < 0 || item.Reserve > item.Quantity)
                throw new ValidationException("Не коректна величина резерву матеріалу", "");
            if (item.Cost <= 0)
                throw new ValidationException("Не коректна вартість матеріалу", "");

            await DataBase.Materials.Create(_mapper.Map<Material>(item));
            await DataBase.SaveAsync();
        }

        public async Task Update(MaterialDTO item)
        {
            if (item.Name == "")
                throw new ValidationException("Порожня назва матеріалу", "");
            if (item.Quantity <= 0)
                throw new ValidationException("Не коректна загальна кількість матерііалу", "");
            if (item.Reserve < 0 || item.Reserve > item.Quantity)
                throw new ValidationException("Не коректна величина резерву матеріалу", "");
            if (item.Cost <= 0)
                throw new ValidationException("Не коректна вартість матеріалу", "");

            Material material = await DataBase.Materials.Get(item.MaterialId);
            if (material == null)
                throw new Exception("Матеріал не знайдено");
            material.MaterialId = item.MaterialId;
            material.Name = item.Name;
            material.Quantity = item.Quantity;
            material.Reserve = item.Reserve;
            material.Cost = item.Cost;
            DataBase.Materials.Update(material);
            await DataBase.SaveAsync();
        }

        public async Task<decimal> GetAvailableQuantity(int id)
        {
            var item = await DataBase.Materials.Get(id);
            if (item == null)
                throw new Exception("Матеріал не знайдено");

            return item.Quantity - item.Reserve;
        }

        public async Task UpdateReserve(int id, decimal value)
        {
            var item = await DataBase.Materials.Get(id);
            if (item == null)
                throw new Exception("Матеріал не знайдено");

            if (item.Reserve + value < 0 || item.Reserve + value > item.Quantity)
                throw new ValidationException("Не коректна величина резерву матеріалу", "");

            item.Reserve += value;
            DataBase.Materials.Update(item);
            await DataBase.SaveAsync();
        }

        public async Task UpdateQuantity(int id, decimal value)
        {
            var item = await DataBase.Materials.Get(id);
            if (item == null)
                throw new Exception("Матеріал не знайдено");

            if (item.Quantity + value < 0)
                throw new ValidationException("Не коректна загальна кількість матерііалу", "");

            item.Quantity += value;
            DataBase.Materials.Update(item);
            await DataBase.SaveAsync();
        }

        public Tuple<List<MaterialDTO>, int> GetMaterials(FilteredMaterialListRequestDTO filter)
        {
            IEnumerable<Material> materials = DataBase.Materials.GetAll();
            if (filter.Name != null) materials = materials.Where(x => x.Name == filter.Name);
            if (filter.Sort != null)
            {
                if (filter.Sort.ToLower() == "desc") materials = materials.OrderByDescending(x => x.Name);
                else
                {
                    if (filter.Sort.ToLower() == "asc") materials = materials.OrderBy(x => x.Name);
                }
            }
            int count = materials.Count();
            if (filter.PageNumber == null) filter.PageNumber = 1;
            if (filter.PageSize == null) { filter.PageSize = 10; };
            if (filter.PageSize > 20) { filter.PageSize = 20; };
            materials = materials.Skip((int)((filter.PageNumber - 1) * filter.PageSize)).Take((int)filter.PageSize);
            return Tuple.Create(_mapper.Map<List<MaterialDTO>>(materials), count);
        }

        public List<string> GetMaterialNames()
        {
            return (DataBase.Materials.GetAll().Select(x => x.Name).Distinct().ToList());
        }

        public List<ResponseIdAndStringDTO> GetDataForSelect()
        {
            var list = DataBase.Materials.GetAll();
            var result = new List<ResponseIdAndStringDTO>();
            foreach (var l in list)
            {
                result.Add(new ResponseIdAndStringDTO() { Id = l.MaterialId, Str = "id: " + l.MaterialId + "; назва: " + l.Name + ";" });
            }
            return result;
        }

        public byte[] ExportMaterials()
        {
            IEnumerable<Material> materials = DataBase.Materials.GetAll();
            var builder = new StringBuilder();
            builder.AppendLine("MaterialId;Name;Quantity;Reserve;Cost");
            foreach (var item in materials)
            {
                builder.AppendLine($"{item.MaterialId};{item.Name};{item.Quantity};{item.Reserve};{item.Cost}");
            }
            return Encoding.UTF8.GetBytes(builder.ToString());
        }

        public void Dispose()
        {
            DataBase.Dispose();
        }
    }
}
