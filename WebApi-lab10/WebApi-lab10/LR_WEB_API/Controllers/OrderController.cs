using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using LR_WEB_API.ActionFilters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace LR_WEB_API.Controllers
{
    [Route("api/warehouses/{warehouseId}/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IDataShaper<OrderDto> _dataShaper;
        public OrderController(IRepositoryManager repository, ILoggerManager logger,
 IMapper mapper, IDataShaper<OrderDto> dataShaper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _dataShaper = dataShaper;
        }
        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetOrderForWarehouse(Guid warehouseId, [FromQuery] OrderParameters orderParameters)
        {
            var warehouse = await _repository.Warehouse.GetWarehouseAsync(warehouseId, trackChanges: false);
            if (warehouse == null)
            {
                _logger.LogInfo($"Company with id: {warehouseId} doesn't exist in the database.");
                return NotFound();
            }
            var orderFromDb = await _repository.Order.GetOrderAsync(warehouseId, orderParameters, trackChanges: false);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(orderFromDb.MetaData));
            var orderDTO = _mapper.Map<IEnumerable<OrderDto>>(orderFromDb);
            return Ok(orderDTO);
        }

        [HttpGet("{id}", Name = "GetOrderForWarehouse")]
        public IActionResult GetOrderForWarehouse(Guid warehouseId, Guid id)
        {
            var warehouse = _repository.Warehouse.GetWarehouse(warehouseId, trackChanges: false);
            if (warehouse == null)
            {
                _logger.LogInfo($"Warehouse with id: {warehouseId} doesn't exist in the database.");
                return NotFound();
            }
            var orderDb = _repository.Order.GetOrder(warehouseId, id, trackChanges: false);
            if (orderDb == null)
            {
                _logger.LogInfo($"Order with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            var order = _mapper.Map<OrderDto>(orderDb);
            return Ok(order);
        }

        [HttpPost]
        public IActionResult CreateOrderForWarehouse(Guid warehouseId, [FromBody]
        OrderForCreationDto order)
        {
            if (order == null)
            {
                _logger.LogError("OrderForCreationDto object sent from client is null.");
            return BadRequest("OrderForCreationDto object is null");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the OderForWarehouseDto object");
                return UnprocessableEntity(ModelState);
            }
            var warehouse = _repository.Warehouse.GetWarehouse(warehouseId, trackChanges: false);
            if (warehouse == null)
            {
                _logger.LogInfo($"Warehouse with id: {warehouseId} doesn't exist in the database.");
            return NotFound();
            }
            var orderEntity = _mapper.Map<Order>(order);
            _repository.Order.CreateOrderForWarehouse(warehouseId, orderEntity);
            _repository.Save();
            var orderToReturn = _mapper.Map<OrderDto>(orderEntity);
            return CreatedAtRoute("GetOrderForWarehouse", new
            {
                warehouseId, id = orderToReturn.Id
            }, orderToReturn);
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateOrderForWarehouseExistsAttribute))]
        public async Task<IActionResult> DeleteOrderForWarehouse(Guid warehouseId, Guid id)

        {
            var orderForWarehouse = HttpContext.Items["order"] as Order;
            _repository.Order.DeleteOrder(orderForWarehouse);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateOrderForWarehouseExistsAttribute))]
        public async Task<IActionResult> UpdateOrderForWarehouse(Guid warehouseId, Guid id,
        [FromBody] OrderForUpdateDto order)
        {
            var orderEntity = HttpContext.Items["order"] as Order;
            _mapper.Map(order, orderEntity);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        [ServiceFilter(typeof(ValidateOrderForWarehouseExistsAttribute))]
        public async Task<IActionResult> PartiallyUpdateOrderForWarehouse(Guid warehouseId,
        Guid id, [FromBody] JsonPatchDocument<OrderForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }
            var orderEntity = HttpContext.Items["order"] as Order;
            var orderToPatch = _mapper.Map<OrderForUpdateDto>(orderEntity);
            patchDoc.ApplyTo(orderToPatch, ModelState);
            TryValidateModel(orderToPatch);
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }
            _mapper.Map(orderToPatch, orderEntity);
            await _repository.SaveAsync();
            return NoContent();
        }
    }
}
