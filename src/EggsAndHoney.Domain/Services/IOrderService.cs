using System.Collections.Generic;
using System.Threading.Tasks;
using EggsAndHoney.Domain.Models;

namespace EggsAndHoney.Domain.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetOrders();
        Task<int> GetNumberOfOrders();
        Task<int> AddOrder(string name, string orderTypeName);
        Task<bool> OrderExists(int orderId);
        Task<IEnumerable<ResolvedOrder>> GetResolvedOrders();
        Task<ResolvedOrder> ResolveOrder(int orderId);
        Task<bool> ResolvedOrderExists(int resolvedOrderId);
        Task<Order> UnresolveOrder(int resolvedOrderId);
    }
}
