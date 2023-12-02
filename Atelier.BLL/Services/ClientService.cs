using Atelier.BLL.DTO;
using Atelier.BLL.Infrastructure;
using Atelier.BLL.Interfaces;
using Atelier.DAL.Entities;
using Atelier.DAL.Interfaces;
using AutoMapper;

namespace Atelier.BLL.Services
{
    public class ClientService : IClientService<ClientDTO>
    {
        IUnitOfWork DataBase { get; set; }
        private readonly IMapper _mapper;

        public ClientService(IUnitOfWork uow, IMapper mapper)
        {
            DataBase = uow;
            _mapper = mapper;
        }

        public List<ClientDTO> GetAll()
        {
            return _mapper.Map<List<ClientDTO>>(DataBase.Clients.GetAll());
        }

        public Tuple<List<ClientDTO>, int> GetClients(FilteredClientListRequestDTO filter)
        {
            IEnumerable<Client> clients = DataBase.Clients.GetAll();
            if (filter.FirstName != null) clients = clients.Where(x => x.FirstName == filter.FirstName);
            if (filter.LastName != null) clients = clients.Where(x => x.LastName == filter.LastName);
            if (filter.Sort != null)
            {
                if (filter.Sort.ToLower() == "desc") clients = clients.OrderByDescending(x => x.FirstName);
                else
                {
                    if (filter.Sort.ToLower() == "asc") clients = clients.OrderBy(x => x.FirstName);
                }
            }
            int count = clients.Count();
            if (filter.PageNumber == null) filter.PageNumber = 1;
            if (filter.PageSize == null) { filter.PageSize = 10; };
            if (filter.PageSize > 20) { filter.PageSize = 20; };
            clients = clients.Skip((int)((filter.PageNumber - 1) * filter.PageSize)).Take((int)filter.PageSize);
            return Tuple.Create(_mapper.Map<List<ClientDTO>>(clients), count);
        }

        public async Task Create(ClientDTO item)
        {
            if (item.FirstName == "")
                throw new ValidationException("Відсутнє ім'я(FirstName) замовника", "");
            if (item.LastName == "")
                throw new ValidationException("Відсутнє прізвище(LastName) замовника", "");
            if (item.PhoneNumber == "")
                throw new ValidationException("Відсутній номер телефону(PhoneNumber) замовника", "");
            var check_phone = DataBase.Clients.Find(x => x.PhoneNumber == item.PhoneNumber);
            if (check_phone.Count() != 0)
                throw new Exception("Замовник з таким номером вже присутній");

            await DataBase.Clients.Create(_mapper.Map<Client>(item));
            await DataBase.SaveAsync();
        }
        public async Task Update(ClientDTO item)
        {
            if (item.FirstName == "")
                throw new ValidationException("Відсутнє ім'я(FirstName) замовника", "");
            if (item.LastName == "")
                throw new ValidationException("Відсутнє прізвище(LastName) замовника", "");
            if (item.PhoneNumber == "")
                throw new ValidationException("Відсутній номер телефону(PhoneNumber) замовника", "");
            var check_phone = DataBase.Clients.Find(x => x.PhoneNumber == item.PhoneNumber);

            Client client = await DataBase.Clients.Get(item.ClientId);
            if (client == null)
                throw new Exception("Client not found");
            client.FirstName = item.FirstName;
            client.MiddleName = item.MiddleName;
            client.LastName = item.LastName;
            client.Address = item.Address;
            client.PhoneNumber = item.PhoneNumber;
            DataBase.Clients.Update(client);
            await DataBase.SaveAsync();
        }

        public List<string> GetFirstNames()
        {
            return (DataBase.Clients.GetAll().Select(x => x.FirstName).Distinct().ToList());
        }

        public List<string> GetLastNames()
        {
            return (DataBase.Clients.GetAll().Select(x => x.LastName).Distinct().ToList());
        }

        public async Task<ClientDTO> GetById(int id)
        {
            var item = await DataBase.Clients.Get(id);
            if (item == null)
                throw new Exception("Клієнта з таким ідентифікатором не знайдено");
            return _mapper.Map<ClientDTO>(item);
        }

        public List<ResponseIdAndStringDTO> GetDataForSelect()
        {
            var list = DataBase.Clients.GetAll();
            var result = new List<ResponseIdAndStringDTO>();
            foreach (var l in list)
            {
                result.Add(new ResponseIdAndStringDTO() { Id = l.ClientId, Str = "id: " + l.ClientId + "; ім'я: " + l.FirstName + "; прізвище: " + l.LastName + ";" });
            }
            return result;
        }

        public void Dispose()
        {
            DataBase.Dispose();
        }
    }
}
