using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using System.Reflection;
using Repository.Extensions.Utility;

namespace Repository.Extensions
{
    public static class RepositoryOrderExtensions
    {
        public static IQueryable<Order> FilterOrder(this IQueryable<Order>
        order) => order;
        public static IQueryable<Order> Search(this IQueryable<Order> order, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return order;
            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return order.Where(e => e.Goods.ToLower().Contains(lowerCaseTerm));
        }
        public static IQueryable<Order> Sort(this IQueryable<Order> order, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return order.OrderBy(e => e.Goods);
            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Employee>(orderByQueryString);
            if (string.IsNullOrWhiteSpace(orderQuery))
                return order.OrderBy(e => e.Goods);
            return order.OrderBy(orderQuery);
        }
    }
}


