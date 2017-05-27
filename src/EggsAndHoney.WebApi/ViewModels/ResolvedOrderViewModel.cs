using System;

namespace EggsAndHoney.WebApi.ViewModels
{
    public class ResolvedOrderViewModel : OrderViewModel
    {
        public DateTime DateResolved { get; }

        public ResolvedOrderViewModel(string name, string order, DateTime datePlaced, DateTime dateResolved)
            : base (name, order, datePlaced)
        {
            DateResolved = dateResolved;
        }
    }
}
