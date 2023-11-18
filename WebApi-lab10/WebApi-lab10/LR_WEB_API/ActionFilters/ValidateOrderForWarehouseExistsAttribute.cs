using Contracts;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace LR_WEB_API.ActionFilters
{
    public class ValidateOrderForWarehouseExistsAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        public ValidateOrderForWarehouseExistsAttribute(IRepositoryManager
        repository,
        ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context,
        ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            var trackChanges = (method.Equals("PUT") || method.Equals("PATCH")) ?
            true : false;
            var warehouseId = (Guid)context.ActionArguments["warehouseId"];
            var warehouse = await _repository.Warehouse.GetWarehouseAsync(warehouseId,
            false);
            if (warehouse == null)
            {
                _logger.LogInfo($"Warehouse with id: {warehouseId} doesn't exist in the database.");
            return;
                context.Result = new NotFoundResult();
            }
            var id = (Guid)context.ActionArguments["id"];
            var order = await _repository.Order.GetOrderAsync(warehouseId, id,
            trackChanges);
            if (order == null)
            {
                _logger.LogInfo($"Order with id: {id} doesn't exist in the database.");
               
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("employee", order);
                await next();
            }
        }
    }
}
