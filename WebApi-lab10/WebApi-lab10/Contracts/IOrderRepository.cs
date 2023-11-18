using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IOrderRepository
    {
        Task<PagedList<Order>> GetOrderAsync(Guid warehouseId, OrderParameters orderParameters, bool trackChanges);
        Task<Order> GetOrderAsync(Guid warehouseId, Guid id, bool trackChanges);
        void CreateOrderForWarehouse(Guid warehouseId, Order order);
        void Deleteorder(Order order);
        object GetOrder(Guid warehouseId, Guid id, bool trackChanges);
        void DeleteOrder(Order? orderForWarehouse);
    }
}
