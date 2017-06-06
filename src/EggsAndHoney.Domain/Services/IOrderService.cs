using System.Collections.Generic;
using EggsAndHoney.Domain.Models;

namespace EggsAndHoney.Domain.Services
{
    public interface IOrderService
    {
        IEnumerable<Order> GetOrders();
        int GetNumberOfOrders();
        int AddOrder(string name, string orderTypeName);
        IEnumerable<ResolvedOrder> GetResolvedOrders();
        ResolvedOrder ResolveOrder(int orderId);
        Order UnresolveOrder(int resolvedOrderId);
    }
}
