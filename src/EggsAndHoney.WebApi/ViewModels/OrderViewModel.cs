using System;

namespace EggsAndHoney.WebApi.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; }
        public string Name { get; }
        public string Order { get; }
        public DateTime DatePlaced { get; }

        public OrderViewModel(int id, string name, string order, DateTime datePlaced)
        {
            Id = id;
            Name = name;
            Order = order;
            DatePlaced = datePlaced;
        }
    }
}
