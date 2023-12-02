using Atelier.BLL.DTO;
using Atelier.BLL.Infrastructure;
using Atelier.BLL.Interfaces;
using Atelier.DAL.Entities;
using Atelier.DAL.Interfaces;
using AutoMapper;
using System.Security.Cryptography;
using System.Text;

namespace Atelier.BLL.Services
{
    public class EmployeeService : IEmployeeService<EmployeeDTO>
    {
        IUnitOfWork DataBase { get; set; }
        private readonly IMapper _mapper;

        public EmployeeService(IUnitOfWork uow, IMapper mapper)
        {
            DataBase = uow;
            _mapper = mapper;
        }

        public List<EmployeeDTO> GetAll()
        {
            return _mapper.Map<List<EmployeeDTO>>(DataBase.Employees.GetAll());
        }

        public async Task<EmployeeDTO> Get(int id)
        {
            var item = await DataBase.Employees.Get(id);
            if (item == null)
                throw new ValidationException("Працівника не знайдено", "");

            return _mapper.Map<EmployeeDTO>(item);
        }

        public async Task Create(EmployeeDTO employee, UserDTO user)
        {
            using (var transaction = await DataBase.BeginTransactionAsync())
            {
                try
                {
                    if (user.Login == "")
                        throw new ValidationException("Не коректний логін", "");
                    if (user.Password == "")
                        throw new ValidationException("Не коректний пароль", "");

                    if (employee.FirstName == "" || employee.LastName == "" || employee.PhoneNumber == "")
                        throw new ValidationException("Не коректна інформація про працівника", "");

                    await CreateUser(user);

                    var created_user = DataBase.Users.Find(x => x.Login == user.Login).FirstOrDefault();
                    employee.UserId = created_user.UserId;
                    await DataBase.Employees.Create(_mapper.Map<Employee>(employee));
                    await DataBase.SaveAsync();

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new Exception($"Помилка на етапі створення користувача чи працівника({e.Message})");
                }
            }
        }

        public async Task Delete(int id)
        {
            using (var transaction = await DataBase.BeginTransactionAsync())
            {
                try
                {
                    var items = DataBase.Orders.Find(x => x.EmployeeId == id);
                    foreach (var i in items)
                    {
                        if (i.Status != DAL.Enums.Status.Completed)
                            throw new Exception("Помилка: Всі замовлення працівника, що видаляється, повинні бути завершеними");
                        i.EmployeeId = null;
                    }
                    var item = await DataBase.Employees.Get(id);
                    await DataBase.Users.Delete(item.UserId);
                    await DataBase.Employees.Delete(id);
                    await DataBase.SaveAsync();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new Exception($"Помилка на етапі видалення працівника({e.Message})");
                }
            }
        }

        private async Task CreateUser(UserDTO item)
        {
            if (item.Login == "")
                throw new ValidationException("Пустий логін користувача", "");
            var user = DataBase.Users.Find(f => f.Login == item.Login).FirstOrDefault();
            if (user != null)
                throw new ValidationException("Існує користувач з таким логіном", "");
            if (item.Password == "")
                throw new ValidationException("Пустий пароль користувача", "");

            item.Password = HashPassowrd(item.Password);
            try
            {
                await DataBase.Users.Create(_mapper.Map<User>(item));
            }
            catch
            {
                throw new Exception("Помилка створення користувача");
            }
            await DataBase.SaveAsync();

            var check_user = DataBase.Users.Find(f => f.Login == item.Login).FirstOrDefault();
            if (check_user == null)
                throw new Exception("Користувача не було створено");
        }

        public static string HashPassowrd(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                return hash;
            }
        }

        public Tuple<List<EmployeeDTO>, int> GetEmployees(FilteredListRequestDTO filter)
        {
            IEnumerable<Employee> employee = DataBase.Employees.GetAll();
            int count = employee.Count();
            if (filter.PageNumber == null) filter.PageNumber = 1;
            if (filter.PageSize == null) { filter.PageSize = 10; };
            if (filter.PageSize > 20) { filter.PageSize = 20; };
            employee = employee.Skip((int)((filter.PageNumber - 1) * filter.PageSize)).Take((int)filter.PageSize);
            return Tuple.Create(_mapper.Map<List<EmployeeDTO>>(employee), count);
        }

        public List<string> GetFirstNames()
        {
            return (DataBase.Employees.GetAll().Select(x => x.FirstName).Distinct().ToList());
        }

        public List<ResponseIdAndStringDTO> GetDataForSelect()
        {
            var list = DataBase.Employees.GetAll();
            var result = new List<ResponseIdAndStringDTO>();
            foreach (var l in list)
            {
                result.Add(new ResponseIdAndStringDTO() { Id = l.EmployeeId, Str = "id: " + l.EmployeeId + "; ім'я: " + l.FirstName + "; прізвище: " + l.LastName + ";" });
            }
            return result;
        }

        public void Dispose()
        {
            DataBase.Dispose();
        }
    }
}
