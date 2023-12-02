using Atelier.BLL.DTO;
using Atelier.BLL.Infrastructure;
using Atelier.BLL.Interfaces;
using Atelier.DAL.Entities;
using Atelier.DAL.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Atelier.BLL.Services
{
    public class UserService : IUserService<UserDTO>
    {
        IUnitOfWork DataBase { get; set; }
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork uow, IMapper mapper)
        {
            DataBase = uow;
            _mapper = mapper;
        }

        public UserDTO GetByLogin(string login)
        {
            var user = DataBase.Users.Find(f => f.Login == login);
            if (user == null)
                throw new ValidationException("Користувача не знайдено", "");

            return _mapper.Map<UserDTO>(user);
        }

        public async Task Create(UserDTO item)
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

            var check_user = DataBase.Users.Find(f => f.Login == item.Login);
            if (check_user != null)
                throw new ValidationException("Користувача не було створено", "");
        }

        public async Task<AuthorizationResponseDTO> Login(UserDTO item, IConfiguration _config)
        {
            try
            {
                if (item.Login == "")
                    throw new ValidationException("Пустий логін користувача", "");
                if (item.Password == "")
                    throw new ValidationException("Пустий пароль користувача", "");

                var user = DataBase.Users.Find(f => f.Login == item.Login);
                if (user.Count == 0)
                    throw new ValidationException("Не коректний логін чи пароль користувача", "");

                if (user.FirstOrDefault().Password != HashPassowrd(item.Password))
                {
                    throw new ValidationException("Не коректний логін чи пароль користувача", "");
                }

                var token = Generate(_mapper.Map<UserDTO>(user.FirstOrDefault()), _config);

                var user_with_employee_data = await DataBase.Users.Get(user.FirstOrDefault().UserId);
                var result = new AuthorizationResponseDTO()
                {
                    Token = token,
                    Id = user_with_employee_data.Employee.UserId,
                    Role = user_with_employee_data.Role,
                    Name = "" + user_with_employee_data.Employee.FirstName + " " + user_with_employee_data.Employee.LastName
                };
                return result;
            }
            catch
            {
                throw new ValidationException("Помилка входу", "");
            }
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

        private string Generate(UserDTO user, IConfiguration _config)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Login),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              claims,
              expires: DateTime.Now.AddMinutes(15),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public void Dispose()
        {
            DataBase.Dispose();
        }
    }
}
