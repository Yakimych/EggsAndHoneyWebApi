using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EggsAndHoney.Domain.Models;

namespace EggsAndHoney.Domain.Services
{
    public class OrderService : IOrderService
    {
        public Task<int> AddOrder(string name, string orderTypeName)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetNumberOfOrders()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Order>> GetOrders()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ResolvedOrder>> GetResolvedOrders()
        {
            throw new NotImplementedException();
        }

        public Task<bool> OrderExists(int orderId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ResolvedOrderExists(int resolvedOrderId)
        {
            throw new NotImplementedException();
        }

        public Task<ResolvedOrder> ResolveOrder(int orderId)
        {
            throw new NotImplementedException();
        }

        public Task<Order> UnresolveOrder(int resolvedOrderId)
        {
            throw new NotImplementedException();
        }
    }
}
