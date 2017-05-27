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

        // GET api/orders
        [HttpGet]
        public IEnumerable<OrderViewModel> Get()
        {
            var orders = _orderService.GetOrders();
            return orders.Select(o => new OrderViewModel(o.Id, o.Name, o.OrderType.Name, o.DatePlaced));
        }

        // GET api/orders/count
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

        [HttpPut("resolve")]
        public void Resolve([FromBody]ResolveUnresolveOrderViewModel resolveUnresolveOrderViewModel)
        {
            _orderService.ResolveOrder(resolveUnresolveOrderViewModel.Id);
        }
    }
}
