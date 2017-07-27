using System;
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
    public class OrdersController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;

        public OrdersController(IMapper mapper, IOrderService orderService)
        {
            _mapper = mapper;
            _orderService = orderService;
        }

        [HttpGet]
		[ProducesResponseType(typeof(ItemCollectionResponseViewModel<OrderViewModel>), 200)]
		public async Task<IActionResult> Get()
        {
            var orders = await _orderService.GetOrders();
            var ordersViewModels = _mapper.Map<IList<OrderViewModel>>(orders.OrderBy(o => o.DatePlaced));
            var responseViewModel = new ItemCollectionResponseViewModel<OrderViewModel>(ordersViewModels.ToList());
            
            return Ok(responseViewModel);
        }

        [HttpGet("count")]
		[ProducesResponseType(typeof(ItemCountResponseViewModel), 200)]
		public async Task<IActionResult> GetCount()
        {
            var numberOfOrders = await _orderService.GetNumberOfOrders();
            var itemCountResponseViewModel = new ItemCountResponseViewModel(numberOfOrders);
            
            return Ok(itemCountResponseViewModel);
        }

        [HttpPost("add")]
		[ProducesResponseType(201)]
		[ProducesResponseType(400)]
		public async Task<IActionResult> Add([FromBody]AddOrderViewModel addOrderViewModel)
        {
            try
            {
                var createdOrderId = await _orderService.AddOrder(addOrderViewModel.Name, addOrderViewModel.Order);
                return StatusCode(201, new { Id = createdOrderId });
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }
        }

        [HttpPost("resolve")]
		[ProducesResponseType(typeof(ResolvedOrderViewModel), 200)]
		[ProducesResponseType(400)]
        [ProducesResponseType(404)]
		public async Task<IActionResult> Resolve([FromBody]ItemIdentifierViewModel itemIdentifier)
        {
            if (!await _orderService.OrderExists(itemIdentifier.Id))
            {
                return NotFound();
            }

            var resolvedOrder = await _orderService.ResolveOrder(itemIdentifier.Id);
            var resolvedOrderViewModel = _mapper.Map<ResolvedOrderViewModel>(resolvedOrder);

			return Ok(resolvedOrderViewModel);
        }
    }
}
