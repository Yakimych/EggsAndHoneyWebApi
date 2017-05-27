using System;

namespace EggsAndHoney.WebApi.ViewModels
{
    public class OrderViewModel
    {
        public string Name { get; }
        public string Order { get; }
        public DateTime DatePlaced { get; }

        public OrderViewModel(string name, string order, DateTime datePlaced)
        {
            Name = name;
            Order = order;
            DatePlaced = datePlaced;
        }
    }
}
