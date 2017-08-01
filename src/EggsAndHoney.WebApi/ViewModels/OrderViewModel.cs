using System;

namespace EggsAndHoney.WebApi.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Order { get; set; }
        public DateTime DatePlaced { get; set; }
    }
}
