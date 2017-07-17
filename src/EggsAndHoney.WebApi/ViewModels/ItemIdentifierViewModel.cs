using System.ComponentModel.DataAnnotations;

namespace EggsAndHoney.WebApi.ViewModels
{
    public class ItemIdentifierViewModel
    {
        [Required, Range(1, int.MaxValue)]
        public int Id { get; set; }
    }
}