using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EggsAndHoney.Domain.Services;
using EggsAndHoney.WebApi.Filters;
using EggsAndHoney.WebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EggsAndHoney.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ValidateModel]
    public class ResolvedOrdersController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;

        public ResolvedOrdersController(IMapper mapper, IOrderService orderService)
        {
            _mapper = mapper;
            _orderService = orderService;
        }

        [HttpGet]
		[ProducesResponseType(typeof(ItemCollectionResponseViewModel<ResolvedOrderViewModel>), 200)]
        public async Task<IActionResult> Get()
        {
            var resolvedOrders = await _orderService.GetResolvedOrders();

            var resolvedOrderViewModels = _mapper.Map<IList<ResolvedOrderViewModel>>(resolvedOrders.OrderByDescending(o => o.DateResolved));
            var itemCollectionResponseViewModel = new ItemCollectionResponseViewModel<ResolvedOrderViewModel>(resolvedOrderViewModels);

            return Ok(itemCollectionResponseViewModel);
        }

        [HttpPost("unresolve")]
		[ProducesResponseType(typeof(OrderViewModel), 200)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
		public async Task<IActionResult> Unresolve([FromBody]ItemIdentifierViewModel itemIdentifier)
        {
			if (itemIdentifier == null)
			{
				return BadRequest("Request body cannot be empty!");
			}

            if (!await _orderService.ResolvedOrderExists(itemIdentifier.Id))
            {
                return NotFound();
            }

            var unresolvedOrder = await _orderService.UnresolveOrder(itemIdentifier.Id);
            var unresolvedOrderViewModel = _mapper.Map<OrderViewModel>(unresolvedOrder);

            return Ok(unresolvedOrderViewModel);
        }
    }
}
