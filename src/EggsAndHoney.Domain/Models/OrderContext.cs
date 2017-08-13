using Microsoft.EntityFrameworkCore;

namespace EggsAndHoney.Domain.Models
{
    public class OrderContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }

        public DbSet<ResolvedOrder> ResolvedOrders { get; set; }

        public DbSet<OrderType> OrderTypes { get; set; }
        
        public OrderContext(DbContextOptions<OrderContext> options)
            : base(options)
        { }
    }
}
