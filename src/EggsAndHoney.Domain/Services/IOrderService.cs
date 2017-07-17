using System.Collections.Generic;
using EggsAndHoney.Domain.Models;

namespace EggsAndHoney.Domain.Services
{
    public interface IOrderService
    {
        IEnumerable<Order> GetOrders();
        int GetNumberOfOrders();
        int AddOrder(string name, string orderTypeName);
        bool OrderExists(int orderId);
        IEnumerable<ResolvedOrder> GetResolvedOrders();
        ResolvedOrder ResolveOrder(int orderId);
        bool ResolvedOrderExists(int resolvedOrderId);
        Order UnresolveOrder(int resolvedOrderId);
    }
}
