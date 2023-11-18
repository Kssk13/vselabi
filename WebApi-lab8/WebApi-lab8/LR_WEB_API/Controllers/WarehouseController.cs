using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using LR_WEB_API.ActionFilters;
using LR_WEB_API.ModelBinders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LR_WEB_API.Controllers
{
    [Route("api/warehouses")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public WarehouseController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;

        }
        [HttpGet]
        public async Task<IActionResult> GetWarehouse()
        {
            var warehouse = await _repository.Warehouse.GetAllWarehouseAsync(trackChanges : false);
            var warehouseDto = _mapper.Map<IEnumerable<WarehouseDto>>(warehouse);
                return Ok(warehouseDto);
        }

        [HttpGet("{id}", Name = "WarehouseById")]
        public async Task<IActionResult> GetWarehouse(Guid id)
        {
           var warehouse = await _repository.Warehouse.GetWarehouseAsync(id, trackChanges:
           false);
            if (warehouse == null)
            {
                _logger.LogInfo($"Warehouse with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            else
            {
                var warehouseDto = _mapper.Map<WarehouseDto>(warehouse);
                return Ok(warehouseDto);
            }
        }


        [HttpGet("collection/({ids})", Name = "WarehouseCollection")]
        public async Task<IActionResult> GetWarehouseCollection(
        [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                _logger.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }
            var warehouseEntities = await _repository.Warehouse.GetByIdsAsync(ids,trackChanges: false);
            if (ids.Count() != warehouseEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound();
            }
            var warehouseToReturn =
           _mapper.Map<IEnumerable<WarehouseDto>>(warehouseEntities);
            return Ok(warehouseToReturn);
        }
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateWarehouse([FromBody] WarehouseForCreationDto
         warehouse)
        {
            var warehouseEntity = _mapper.Map<Warehouse>(warehouse);
            _repository.Warehouse.CreateWarehouse(warehouseEntity);
            await _repository.SaveAsync();
         var warehouseToReturn = _mapper.Map<WarehouseDto>(warehouseEntity);
            return CreatedAtRoute("WarehouseById", new { id = warehouseToReturn.Id },
            warehouseToReturn);
        }

        [HttpPost("collection")]
        public async Task<IActionResult> CreateWarehouseCollection(
        [FromBody] IEnumerable<WarehouseForCreationDto> warehouseCollection)
        {
            if (warehouseCollection == null)
            {
                _logger.LogError("Warehouse collection sent from client is null.");
                return BadRequest("Warehouse collection is null");
            }
            var warehouseEntities = _mapper.Map<IEnumerable<Warehouse>>(warehouseCollection);
            foreach (var warehouse in warehouseEntities)
            {
                _repository.Warehouse.CreateWarehouse(warehouse);
            }
            await _repository.SaveAsync();
            var warehouseCollectionToReturn =
            _mapper.Map<IEnumerable<WarehouseDto>>(warehouseEntities);
            var ids = string.Join(",", warehouseCollectionToReturn.Select(c => c.Id));
            return CreatedAtRoute("WarehouseCollection", new { ids },
            warehouseCollectionToReturn);
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateWarehouseExistsAttribute))]
        public async Task<IActionResult> DeleteWarehouse(Guid id)
        {
            var warehouse = HttpContext.Items["warehouse"] as Warehouse;
            if (warehouse == null)
            {
                _logger.LogInfo($"Warehouse with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            _repository.Warehouse.DeleteWarehouse(warehouse);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateWarehouseExistsAttribute))]
        public async Task<IActionResult> UpdateWarehouse(Guid id, [FromBody]
        WarehouseForUpdateDto
        warehouse)
        {
            var warehouseEntity = HttpContext.Items["warehouse"] as Warehouse;
            if (warehouseEntity == null)
            {
                _logger.LogInfo($"Warehouse with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            _mapper.Map(warehouse, warehouseEntity);
            await _repository.SaveAsync();
            return NoContent();
        }
    }
}
