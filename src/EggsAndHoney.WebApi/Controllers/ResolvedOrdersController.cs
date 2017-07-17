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
		[ProducesResponseType(typeof(ItemCollectionResponseViewModel<ResolvedOrderViewModel>), 200)]
        public IActionResult Get()
        {
            var resolvedOrders = _orderService.GetResolvedOrders();
            var resolvedOrderViewModels = resolvedOrders.
                                            Select(o => new ResolvedOrderViewModel(o.Id, o.Name, o.OrderType.Name, o.DatePlaced, o.DateResolved)).
                                            OrderByDescending(o => o.DateResolved);
            var itemCollectionResponseViewModel = new ItemCollectionResponseViewModel<ResolvedOrderViewModel>(resolvedOrderViewModels.ToList());

            return Ok(itemCollectionResponseViewModel);
        }

        [HttpPost("unresolve")]
		[ProducesResponseType(typeof(OrderViewModel), 200)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
		public IActionResult Unresolve([FromBody]ItemIdentifierViewModel itemIdentifier)
        {
            if (itemIdentifier == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!_orderService.ResolvedOrderExists(itemIdentifier.Id))
            {
                return NotFound();
            }

            var unresolvedOrder = _orderService.UnresolveOrder(itemIdentifier.Id);
            var unresolvedOrderViewModel = new OrderViewModel(unresolvedOrder.Id, unresolvedOrder.Name, unresolvedOrder.OrderType.Name, unresolvedOrder.DatePlaced);

            return Ok(unresolvedOrderViewModel);
        }
    }
}
