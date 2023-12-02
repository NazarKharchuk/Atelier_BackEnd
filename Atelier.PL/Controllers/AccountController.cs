using Atelier.BLL.DTO;
using Atelier.BLL.Interfaces;
using Atelier.PL.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Atelier.PL.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        readonly IUserService<UserDTO> userService;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public AccountController(IUserService<UserDTO> serv, IConfiguration config, IMapper mapper)
        {
            userService = serv;
            _config = config;
            _mapper = mapper;
        }

        //[AllowAnonymous]
        [Route("api/login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLoginModel userLogin)
        {
            try
            {
                var res = await userService.Login(_mapper.Map<UserDTO>(userLogin), _config);

                if (res != null && res.Token != "")
                {
                    return new ObjectResult(new ResponseModel<AuthorizationResponseModel>()
                    {
                        Data = _mapper.Map<AuthorizationResponseModel>(res),
                        Seccessfully = true
                    });
                }

                return new ObjectResult(new ResponseModel<AuthorizationResponseModel>()
                {
                    Seccessfully = false,
                    Message = "Користувача не знайдено"
                });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new ResponseModel<AuthorizationResponseModel>()
                {
                    Seccessfully = false,
                    Message = ex.Message,
                    Code = 500
                });
            }
        }
    }
}
