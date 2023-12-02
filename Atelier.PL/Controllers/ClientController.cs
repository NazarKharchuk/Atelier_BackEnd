using Atelier.BLL.DTO;
using Atelier.BLL.Interfaces;
using Atelier.PL.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Atelier.PL.Controllers
{
    //[Authorize]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService<ClientDTO> clientService;
        private readonly IMapper _mapper;

        public ClientController(IClientService<ClientDTO> serv, IMapper mapper)
        {
            clientService = serv;
            _mapper = mapper;
        }

        [Route("api/clients")]
        [HttpGet]
        public IActionResult GetClients([FromQuery] FilteredClientListRequestModel filter)
        {
            var res = clientService.GetClients(_mapper.Map<FilteredClientListRequestDTO>(filter));
            return new ObjectResult(new ResponseModel<FilteredListResponseModel<ClientModel>>()
            {
                Data = new FilteredListResponseModel<ClientModel>() { List = _mapper.Map<List<ClientModel>>(res.Item1), TotalCount = res.Item2 },
                Seccessfully = true
            });
        }

        [Route("api/clients/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetClient(int id)
        {
            try
            {
                var item = await clientService.GetById(id);

                return new ObjectResult(new ResponseModel<ClientModel>()
                {
                    Seccessfully = true,
                    Data = _mapper.Map<ClientModel>(item)
                });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new ResponseModel<ClientModel>() { Seccessfully = false, Code = 404, Message = ex.Message });
            }

        }

        [Route("api/clients")]
        [HttpPost]
        public async Task<IActionResult> PostClient([FromBody] ClientModel item)
        {
            try
            {
                await clientService.Create(_mapper.Map<ClientDTO>(item));

                return new ObjectResult(new ResponseModel<ClientModel>()
                {
                    Seccessfully = true
                });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new ResponseModel<ClientModel>() { Seccessfully = false, Code = 400, Message = ex.Message });
            }
        }

        [Route("api/clients/{id}")]
        [HttpPut]
        public async Task<IActionResult> PutClient(int id, [FromBody] ClientModel item)
        {
            if (id != item.ClientId)
            {
                return new ObjectResult(new ResponseModel<ClientModel>() { Seccessfully = false, Code = 400, Message = "Різні ідентифікатори" });
            }

            try
            {
                await clientService.Update(_mapper.Map<ClientDTO>(item));

                return new ObjectResult(new ResponseModel<ClientModel>() { Seccessfully = true });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new ResponseModel<ClientModel>() { Seccessfully = false, Code = 404, Message = ex.Message });
            }
        }

        [Route("api/clients/firstNames")]
        [HttpGet]
        public IActionResult GetFirstNames()
        {
            return new ObjectResult(new ResponseModel<List<string>>() { Seccessfully = true, Data = clientService.GetFirstNames() });
        }

        [Route("api/clients/lastNames")]
        [HttpGet]
        public IActionResult GetLastNames()
        {
            return new ObjectResult(new ResponseModel<List<string>>() { Seccessfully = true, Data = clientService.GetLastNames() });
        }

        [Route("api/clients/selectData")]
        [HttpGet]
        public IActionResult GetDataForSelect()
        {
            return new ObjectResult(new ResponseModel<List<ResponseIdAndStringModel>>() { Seccessfully = true, Data = _mapper.Map<List<ResponseIdAndStringModel>>(clientService.GetDataForSelect()) });
        }
    }
}
