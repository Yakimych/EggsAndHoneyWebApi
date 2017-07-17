﻿using System.Linq;
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
		[ProducesResponseType(typeof(ItemCollectionResponseViewModel<OrderViewModel>), 200)]
		public IActionResult Get()
        {
            var orders = _orderService.GetOrders();
            var ordersViewModels = orders.Select(o => new OrderViewModel(o.Id, o.Name, o.OrderType.Name, o.DatePlaced)).OrderBy(o => o.DatePlaced);
            var responseViewModel = new ItemCollectionResponseViewModel<OrderViewModel>(ordersViewModels.ToList());
            
            return Ok(responseViewModel);
        }

        [HttpGet("count")]
		[ProducesResponseType(typeof(ItemCountResponseViewModel), 200)]
		public IActionResult GetCount()
        {
            var numberOfOrders =  _orderService.GetNumberOfOrders();
            var itemCountResponseViewModel = new ItemCountResponseViewModel(numberOfOrders);
            
            return Ok(itemCountResponseViewModel);
        }

        [HttpPost("add")]
		[ProducesResponseType(201)]
		[ProducesResponseType(400)]
		public IActionResult Add([FromBody]AddOrderViewModel addOrderViewModel)
        {
            if (addOrderViewModel == null || !ModelState.IsValid)
            {
                return BadRequest();
            }
            
            var createdOrderId = _orderService.AddOrder(addOrderViewModel.Name, addOrderViewModel.Order);
            
            return StatusCode(201, new { Id = createdOrderId });
        }

        [HttpPost("resolve")]
		[ProducesResponseType(typeof(ResolvedOrderViewModel), 200)]
		[ProducesResponseType(400)]
        [ProducesResponseType(404)]
		public IActionResult Resolve([FromBody]ItemIdentifierViewModel itemIdentifier)
        {
            if (itemIdentifier == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!_orderService.OrderExists(itemIdentifier.Id))
            {
                return NotFound();
            }

            var resolvedOrder = _orderService.ResolveOrder(itemIdentifier.Id);
            var resolvedOrderViewModel = new ResolvedOrderViewModel(resolvedOrder.Id, resolvedOrder.Name, resolvedOrder.OrderType.Name, resolvedOrder.DatePlaced, resolvedOrder.DateResolved);
            
            return Ok(resolvedOrderViewModel);
        }
    }
}
