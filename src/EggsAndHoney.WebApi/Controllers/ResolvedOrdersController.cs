using System;
using System.Collections.Generic;
using EggsAndHoney.WebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EggsAndHoney.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    public class ResolvedOrdersController : Controller
    {
        // GET api/resolvedorders
        [HttpGet]
        public IEnumerable<ResolvedOrderViewModel> Get()
        {
            var order1 = new ResolvedOrderViewModel("YaK", "Eggs", DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(-1).AddHours(1));
            var order2 = new ResolvedOrderViewModel("Rita", "Honey", DateTime.UtcNow, DateTime.UtcNow.AddHours(1));
            return new [] { order1, order2 };
        }
    }
}
