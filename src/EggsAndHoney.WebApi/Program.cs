using System;
using System.Linq;
using EggsAndHoney.Domain.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace EggsAndHoney.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webhost = BuildWebHost(args);
            
            EnsureInMemoryDataExists(webhost.Services);
                
            webhost.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();

        public static void EnsureInMemoryDataExists(IServiceProvider serviceProvider)
        {
            using (var context = (OrderContext)serviceProvider.GetService(typeof(OrderContext)))
            {
                var orderTypeSet = context.Set<OrderType>();
                var existingOrderTypes = orderTypeSet.ToList();
                
                if (!existingOrderTypes.Any(t => t.Name == "Eggs"))
                    orderTypeSet.Add(new OrderType { Id = 1, Name = "Eggs" });
                if (!existingOrderTypes.Any(t => t.Name == "Honey"))
                    orderTypeSet.Add(new OrderType { Id = 2, Name = "Honey" });
                
                context.SaveChanges();
            }
        }
    }
}
