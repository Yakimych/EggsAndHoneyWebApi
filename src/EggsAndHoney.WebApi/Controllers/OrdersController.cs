using System.Collections.Generic;
using System.Linq;
using EggsAndHoney.Domain.Services;
using EggsAndHoney.WebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EggsAndHoney.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public IEnumerable<OrderViewModel> Get()
        {
            var orders = _orderService.GetOrders();
            return orders.Select(o => new OrderViewModel(o.Id, o.Name, o.OrderType.Name, o.DatePlaced)).OrderBy(o => o.DatePlaced);
        }

        [HttpGet("count")]
        public int GetCount()
        {
            return _orderService.GetNumberOfOrders();
        }

        [HttpPost("add")]
        public void Add([FromBody]AddOrderViewModel addOrderViewModel)
        {
            _orderService.AddOrder(addOrderViewModel.Name, addOrderViewModel.Order);
        }

        [HttpPut("resolve/{id}")]
        public ResolvedOrderViewModel Resolve(int id)
        {
            var resolvedOrder = _orderService.ResolveOrder(id);
            return new ResolvedOrderViewModel(resolvedOrder.Id, resolvedOrder.Name, resolvedOrder.OrderType.Name, resolvedOrder.DatePlaced, resolvedOrder.DateResolved);
        }
    }
}
