using System;
using System.ComponentModel.DataAnnotations;

namespace EggsAndHoney.Domain.Models
{
    public class ResolvedOrder
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public DateTime DatePlaced { get; set; }

        public DateTime DateResolved { get; set; }

        [Required]
        public OrderType OrderType { get; set; }

        public ResolvedOrder(int id, string name, OrderType orderType, DateTime datePlaced, DateTime dateResolved)
        {
            Id = id;
            Name = name;
            OrderType = orderType;
            DatePlaced = datePlaced;
            DateResolved = dateResolved;
        }

        public ResolvedOrder(int id, Order order, DateTime dateResolved)
            : this(id, order.Name, order.OrderType, order.DatePlaced, dateResolved)
        { }
    }
}
