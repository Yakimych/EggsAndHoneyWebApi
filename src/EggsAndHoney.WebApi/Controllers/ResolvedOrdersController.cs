using System.Collections.Generic;
using System.Linq;
using EggsAndHoney.Domain.Services;
using EggsAndHoney.WebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EggsAndHoney.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    public class ResolvedOrdersController : Controller
    {
        private readonly IOrderService _orderService;

        public ResolvedOrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public IEnumerable<ResolvedOrderViewModel> Get()
        {
            var resolvedOrders = _orderService.GetResolvedOrders();
            return resolvedOrders.Select(o => new ResolvedOrderViewModel(o.Id, o.Name, o.OrderType.Name, o.DatePlaced, o.DateResolved));
        }

        [HttpPut("unresolve/{id}")]
        public OrderViewModel Unresolve(int id)
        {
            var unresolvedOrder = _orderService.UnresolveOrder(id);
            return new OrderViewModel(unresolvedOrder.Id, unresolvedOrder.Name, unresolvedOrder.OrderType.Name, unresolvedOrder.DatePlaced);
        }
    }
}
