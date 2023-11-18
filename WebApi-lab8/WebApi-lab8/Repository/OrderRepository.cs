using Contracts;
using Entities.Models;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.RequestFeatures;
using System.ComponentModel.Design;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }
        public async Task<PagedList<Order>> GetOrderAsync(Guid portsId, OrderParameters orderParameters, bool trackChanges)
        {
            var order = await FindByCondition(e => e.WarehouseId.Equals(portsId),
            trackChanges)
            .OrderBy(e => e.Goods)
            .ToListAsync();
            return PagedList<Order>
            .ToPagedList(order, orderParameters.PageNumber,
            orderParameters.PageSize);
        }
        public async Task<Order> GetShipAsync(Guid warehouseId, Guid id, bool trackChanges) =>
        await FindByCondition(e => e.WarehouseId.Equals(warehouseId) && e.Id.Equals(id), trackChanges)
            .SingleOrDefaultAsync();
        public void CreateOrderForWarehouse(Guid warehouseId, Order order)
        {
            order.WarehouseId = warehouseId;
            Create(order);
        }
        public void Deleteorder(Order order)
        {
            Delete(order);
        }

        public Task<Order> GetOrderAsync(Guid warehouseId, Guid id, bool trackChanges)
        {
            throw new NotImplementedException();
        }

        public object GetOrder(Guid warehouseId, Guid id, bool trackChanges)
        {
            throw new NotImplementedException();
        }

        public void DeleteOrder(Order? orderForWarehouse)
        {
            throw new NotImplementedException();
        }
    }
}
