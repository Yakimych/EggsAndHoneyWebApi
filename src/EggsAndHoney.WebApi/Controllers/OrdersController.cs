using System;
using System.Collections.Generic;
using EggsAndHoney.WebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EggsAndHoney.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    public class OrdersController : Controller
    {
        // GET api/orders
        [HttpGet]
        public IEnumerable<OrderViewModel> Get()
        {
            var order1 = new OrderViewModel("YaK", "Eggs", DateTime.UtcNow.AddDays(-1));
            var order2 = new OrderViewModel("Rita", "Honey", DateTime.UtcNow);
            return new [] { order1, order2 };
        }

        // GET api/orders/count
        [HttpGet("count")]
        public int GetCount()
        {
            return 2;
        }
    }
}
