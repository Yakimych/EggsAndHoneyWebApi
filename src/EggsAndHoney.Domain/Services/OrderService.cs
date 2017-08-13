using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EggsAndHoney.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EggsAndHoney.Domain.Services
{
    public class OrderService : IOrderService
    {
        private readonly OrderContext _dbContext;
        private readonly DbSet<Order> _orderSet;
        private readonly DbSet<ResolvedOrder> _resolvedOrderSet;
        private readonly DbSet<OrderType> _orderTypeSet;

        public OrderService(OrderContext orderContext)
        {
            _dbContext = orderContext;
            _orderSet = _dbContext.Set<Order>();
            _resolvedOrderSet = _dbContext.Set<ResolvedOrder>();
            _orderTypeSet = _dbContext.Set<OrderType>();
        }

        public async Task<int> AddOrder(string name, string orderTypeName)
        {
            var orderTypeSet = _dbContext.Set<OrderType>();
            var orderType = orderTypeSet.Single(t => t.Name == orderTypeName);

            if (_orderSet.Any(o => o.Name == name && o.OrderType.Id == orderType.Id))
                throw new InvalidOperationException($"An order for {orderTypeName} has already been placed by {name}.");

            var addedOrder = await _orderSet.AddAsync(new Order(name, orderType, DateTime.UtcNow));
            await _dbContext.SaveChangesAsync();
            return addedOrder.Entity.Id;
        }

        public async Task<int> GetNumberOfOrders()
        {
            return await _orderSet.CountAsync();
        }

        public async Task<IEnumerable<Order>> GetOrders()
        {
            return await _orderSet.Include(o => o.OrderType).ToListAsync();
        }

        public async Task<IEnumerable<ResolvedOrder>> GetResolvedOrders()
        {
            return  await _resolvedOrderSet.Include(o => o.OrderType).ToListAsync();
        }

        public async Task<bool> OrderExists(int orderId)
        {
            return await _orderSet.AnyAsync(o => o.Id == orderId);
        }

        public async Task<bool> ResolvedOrderExists(int resolvedOrderId)
        {
            return await _resolvedOrderSet.AnyAsync(o => o.Id == resolvedOrderId);
        }

        public async Task<ResolvedOrder> ResolveOrder(int orderId)
        {
            var orderToResolve = await _orderSet.Include(o => o.OrderType).SingleAsync(o => o.Id == orderId);
            var newResolvedOrder = new ResolvedOrder(orderToResolve, DateTime.UtcNow);

            _orderSet.Remove(orderToResolve);
            var resolvedOrder = await _resolvedOrderSet.AddAsync(newResolvedOrder);
            await _dbContext.SaveChangesAsync();

            return resolvedOrder.Entity;
        }

        public async Task<Order> UnresolveOrder(int resolvedOrderId)
        {
            var orderToUnresolve = await _resolvedOrderSet.Include(o => o.OrderType).SingleAsync(o => o.Id == resolvedOrderId);
            var newUnresolvedOrder = new Order(orderToUnresolve);

            _resolvedOrderSet.Remove(orderToUnresolve);
            var unresolvedOrder = await _orderSet.AddAsync(newUnresolvedOrder);
            await _dbContext.SaveChangesAsync();

            return unresolvedOrder.Entity;
        }
    }
}
