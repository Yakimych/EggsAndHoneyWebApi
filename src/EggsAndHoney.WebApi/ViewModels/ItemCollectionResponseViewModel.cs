using System.Collections.Generic;

namespace EggsAndHoney.WebApi.ViewModels
{
    public class ItemCollectionResponseViewModel<T>
    {
        public IList<T> Items { get; }
        
        public ItemCollectionResponseViewModel(IList<T> items)
        {
            Items = items;
        }

    }
}