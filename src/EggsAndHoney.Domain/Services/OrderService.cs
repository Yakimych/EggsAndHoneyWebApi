using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EggsAndHoney.Domain.Models;

namespace EggsAndHoney.Domain.Services
{
    public class OrderService : IOrderService
    {
        private static int __currentFakeOrderId = 1;
        private static int __currentFakeResolvedOrderId = 1;

        private static readonly List<OrderType> __fakeOrderTypes = new List<OrderType>
        {
            new OrderType { Id = 1, Name = "Eggs" },
            new OrderType { Id = 2, Name = "Honey" }
        };

        private static readonly List<Order> __fakeOrders = new List<Order>
        {
            new Order(__currentFakeOrderId++, "FakeName1", __fakeOrderTypes.First(), DateTime.UtcNow.AddDays(-3)),
            new Order(__currentFakeOrderId++, "FakeName1", __fakeOrderTypes.Last(), DateTime.UtcNow.AddDays(-2)),
            new Order(__currentFakeOrderId++, "FakeName2", __fakeOrderTypes.First(), DateTime.UtcNow.AddDays(-1))
        };

        private static readonly List<ResolvedOrder> __fakeResolvedOrders = new List<ResolvedOrder>
        {
            new ResolvedOrder(__currentFakeResolvedOrderId++, "FakeName1", __fakeOrderTypes.First(), DateTime.UtcNow.AddDays(-4), DateTime.UtcNow.AddDays(-4).AddHours(1))
        };

        public async Task<IEnumerable<Order>> GetOrders()
        {
            return __fakeOrders;
        }

        public async Task<int> GetNumberOfOrders()
        {
            return __fakeOrders.Count;
        }

        public async Task<int> AddOrder(string name, string orderTypeName)
        {
            var orderType = __fakeOrderTypes.Single(o => o.Name == orderTypeName);

            if (__fakeOrders.Any(o => o.Name == name && o.OrderType.Id == orderType.Id))
                throw new InvalidOperationException($"An order for {orderTypeName} has already been placed by {name}.");

            var orderToAdd = new Order(__currentFakeOrderId++, name, orderType, DateTime.UtcNow);

            __fakeOrders.Add(orderToAdd);

            return orderToAdd.Id;
        }
        
        public async Task<bool> OrderExists(int orderId)
        {
            return __fakeOrders.Any(o => o.Id == orderId);
        }

        public async Task<IEnumerable<ResolvedOrder>> GetResolvedOrders()
        {
            return __fakeResolvedOrders;
        }

        public async Task<ResolvedOrder> ResolveOrder(int orderId)
        {
            var orderToResolve = __fakeOrders.Single(o => o.Id == orderId);
            var resolvedOrder = new ResolvedOrder(__currentFakeResolvedOrderId++, orderToResolve, DateTime.UtcNow);

            __fakeOrders.Remove(orderToResolve);
            __fakeResolvedOrders.Add(resolvedOrder);

            return resolvedOrder;
        }

        public async Task<Order> UnresolveOrder(int resolvedOrderId)
        {
            var orderToUnresolve = __fakeResolvedOrders.Single(o => o.Id == resolvedOrderId);
            var unresolvedorder = new Order(__currentFakeOrderId++, orderToUnresolve);

            __fakeResolvedOrders.Remove(orderToUnresolve);
            __fakeOrders.Add(unresolvedorder);

            return unresolvedorder;
        }

        public async Task<bool> ResolvedOrderExists(int resolvedOrderId)
        {
            return __fakeResolvedOrders.Any(o => o.Id == resolvedOrderId);
        }
    }
}
