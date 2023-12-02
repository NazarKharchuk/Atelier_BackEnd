using Atelier.BLL.DTO;
using Atelier.BLL.Interfaces;
using Atelier.PL.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Atelier.PL.Controllers
{
    //[Authorize(Roles = "Admin")]
    [ApiController]
    public class WorksTypeController : ControllerBase
    {
        private readonly IWorksTypeService<WorksTypeDTO> worksTypeService;
        private readonly IMapper _mapper;

        public WorksTypeController(IWorksTypeService<WorksTypeDTO> serv, IMapper mapper)
        {
            worksTypeService = serv;
            _mapper = mapper;
        }

        [Route("api/worksTypes")]
        [HttpGet]
        public IActionResult GetWorksTypes([FromQuery] FilteredListRequestModel filter)
        {
            var res = worksTypeService.GetWorksTypes(_mapper.Map<FilteredListRequestDTO>(filter));
            return new ObjectResult(new ResponseModel<FilteredListResponseModel<WorksTypeModel>>()
            {
                Data = new FilteredListResponseModel<WorksTypeModel>() { List = _mapper.Map<List<WorksTypeModel>>(res.Item1), TotalCount = res.Item2 },
                Seccessfully = true
            });
        }

        [Route("api/worksTypes/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetWorksType(int id)
        {
            try
            {
                var item = await worksTypeService.GetById(id);

                return new ObjectResult(new ResponseModel<WorksTypeModel>()
                {
                    Seccessfully = true,
                    Data = _mapper.Map<WorksTypeModel>(item)
                });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new ResponseModel<WorksTypeModel>() { Seccessfully = false, Code = 404, Message = ex.Message });
            }

        }

        [Route("api/worksTypes")]
        [HttpPost]
        public async Task<IActionResult> PostWorksType([FromBody] WorksTypeModel item)
        {
            try
            {
                await worksTypeService.Create(_mapper.Map<WorksTypeDTO>(item));

                return new ObjectResult(new ResponseModel<WorksTypeModel>() { Seccessfully = true });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new ResponseModel<WorksTypeModel>() { Seccessfully = false, Code = 400, Message = ex.Message });
            }
        }

        [Route("api/worksTypes/{id}")]
        [HttpPut]
        public async Task<IActionResult> PutWorksType(int id, [FromBody] WorksTypeModel item)
        {
            if (id != item.WorksTypeId)
            {
                return new ObjectResult(new ResponseModel<WorksTypeModel>() { Seccessfully = false, Code = 400, Message = "Різні ідентифікатори" });
            }

            try
            {
                await worksTypeService.Update(_mapper.Map<WorksTypeDTO>(item));

                return new ObjectResult(new ResponseModel<WorksTypeModel>() { Seccessfully = true });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new ResponseModel<WorksTypeModel>() { Seccessfully = false, Code = 404, Message = ex.Message });
            }
        }

        //[Authorize(Roles = "Admin, User")]
        [Route("api/worksTypes/names")]
        [HttpGet]
        public IActionResult GetNames()
        {
            return new ObjectResult(new ResponseModel<List<string>>() { Seccessfully = true, Data = worksTypeService.GetNames() });
        }

        //[Authorize(Roles = "Admin, User")]
        [Route("api/worksTypes/selectData")]
        [HttpGet]
        public IActionResult GetDataForSelect()
        {
            return new ObjectResult(new ResponseModel<List<ResponseIdAndStringModel>>() { Seccessfully = true, Data = _mapper.Map<List<ResponseIdAndStringModel>>(worksTypeService.GetDataForSelect()) });
        }
    }
}
