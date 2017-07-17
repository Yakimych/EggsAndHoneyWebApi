namespace EggsAndHoney.WebApi.ViewModels
{
    public class ItemCountResponseViewModel
    {
        public int Count { get; }

        public ItemCountResponseViewModel(int count)
        {
            Count = count;        
        }
    }
}