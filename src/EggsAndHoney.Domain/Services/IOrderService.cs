﻿using System.Collections.Generic;
using EggsAndHoney.Domain.Models;

namespace EggsAndHoney.Domain.Services
{
    public interface IOrderService
    {
        IEnumerable<Order> GetOrders();
        int GetNumberOfOrders();
        int AddOrder(string name, string orderTypeName);
        IEnumerable<ResolvedOrder> GetResolvedOrders();
        int ResolveOrder(int orderId);
        int UnresolveOrder(int resolvedOrderId);
    }
}
