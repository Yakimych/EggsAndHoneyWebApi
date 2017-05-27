using System;

namespace EggsAndHoney.WebApi.ViewModels
{
    public class ResolvedOrderViewModel : OrderViewModel
    {
        public DateTime DateResolved { get; }

        public ResolvedOrderViewModel(int id, string name, string order, DateTime datePlaced, DateTime dateResolved)
            : base (id, name, order, datePlaced)
        {
            DateResolved = dateResolved;
        }
    }
}
