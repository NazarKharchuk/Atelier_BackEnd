using Atelier.BLL.DTO;
using Atelier.BLL.Interfaces;
using Atelier.PL.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Atelier.PL.Controllers
{
    [Authorize]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly IMaterialService<MaterialDTO> materialService;
        private readonly IMapper _mapper;

        public MaterialController(IMaterialService<MaterialDTO> serv, IMapper mapper)
        {
            materialService = serv;
            _mapper = mapper;
        }

        [Route("api/materials")]
        [HttpGet]
        public IActionResult GetMaterials([FromQuery] FilteredMaterialListRequestModel filter)
        {
            var res = materialService.GetMaterials(_mapper.Map<FilteredMaterialListRequestDTO>(filter));
            return new ObjectResult(new ResponseModel<FilteredListResponseModel<MaterialModel>>()
            {
                Data = new FilteredListResponseModel<MaterialModel>() { List = _mapper.Map<List<MaterialModel>>(res.Item1), TotalCount = res.Item2 },
                Seccessfully = true
            });
        }

        [Route("api/materials/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetMaterial(int id)
        {
            try
            {
                var item = await materialService.Get(id);

                return new ObjectResult(new ResponseModel<MaterialModel>()
                {
                    Seccessfully = true,
                    Data = _mapper.Map<MaterialModel>(item)
                });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new ResponseModel<MaterialModel>() { Seccessfully = false, Code = 404, Message = ex.Message });
            }

        }

        [Route("api/materials")]
        [HttpPost]
        public async Task<IActionResult> PostMaterial([FromBody] MaterialModel item)
        {
            try
            {
                await materialService.Create(_mapper.Map<MaterialDTO>(item));

                return new ObjectResult(new ResponseModel<MaterialModel>()
                {
                    Seccessfully = true
                });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new ResponseModel<MaterialModel>() { Seccessfully = false, Code = 400, Message = ex.Message });
            }
        }

        [Route("api/materials/{id}")]
        [HttpPut]
        public async Task<IActionResult> PutMaterial(int id, [FromBody] MaterialModel item)
        {
            if (id != item.MaterialId)
            {
                return new ObjectResult(new ResponseModel<MaterialModel>() { Seccessfully = false, Code = 400, Message = "Різні ідентифікатори" });
            }

            try
            {
                await materialService.Update(_mapper.Map<MaterialDTO>(item));

                return new ObjectResult(new ResponseModel<MaterialModel>() { Seccessfully = true });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new ResponseModel<MaterialModel>() { Seccessfully = false, Code = 404, Message = ex.Message });
            }
        }

        [Route("api/materials/quantity/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetAvailableQuantity(int id)
        {
            try
            {
                var item = await materialService.GetAvailableQuantity(id);

                return new ObjectResult(new ResponseModel<Decimal>() { Seccessfully = true, Data = item });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new ResponseModel<Decimal>() { Seccessfully = false, Code = 404, Message = ex.Message });
            }
        }

        [Route("api/materials/reserve/{id}")]
        [HttpPut]
        public async Task<IActionResult> PutReserve(int id, [FromBody] decimal count)
        {
            try
            {
                await materialService.UpdateReserve(id, count);

                return new ObjectResult(new ResponseModel<Decimal>() { Seccessfully = true });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new ResponseModel<Decimal>() { Seccessfully = false, Code = 404, Message = ex.Message });
            }
        }

        [Route("api/materials/names")]
        [HttpGet]
        public IActionResult GetNames()
        {
            return new ObjectResult(new ResponseModel<List<string>>() { Seccessfully = true, Data = materialService.GetMaterialNames() });
        }

        [Route("api/materials/selectData")]
        [HttpGet]
        public IActionResult GetDataForSelect()
        {
            return new ObjectResult(new ResponseModel<List<MaterialModel>>() { Seccessfully = true, Data = _mapper.Map<List<MaterialModel>>(materialService.GetAll()) });
        }

        [Route("api/materials/exportData")]
        [HttpGet]
        public IActionResult GetExportMaterials()
        {
            var bytes = materialService.ExportMaterials();
            return File(bytes, "text/csv", "Materials.csv");
        }
    }
}
