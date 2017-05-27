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

        // GET api/v1/resolvedorders
        [HttpGet]
        public IEnumerable<ResolvedOrderViewModel> Get()
        {
            var resolvedOrders = _orderService.GetResolvedOrders();
            return resolvedOrders.Select(o => new ResolvedOrderViewModel(o.Id, o.Name, o.OrderType.Name, o.DatePlaced, o.DateResolved));
        }
    }
}
