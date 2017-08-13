﻿using System;
using System.ComponentModel.DataAnnotations;

namespace EggsAndHoney.Domain.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public DateTime DatePlaced { get; set; }

        [Required]
        public OrderType OrderType { get; set; }

        public Order() { }

        public Order(string name, OrderType orderType, DateTime datePlaced)
        {
            Name = name;
            OrderType = orderType;
            DatePlaced = datePlaced;
        }

        public Order(ResolvedOrder resolvedOrder)
            : this(resolvedOrder.Name, resolvedOrder.OrderType, resolvedOrder.DatePlaced)
        { }
    }
}
