using System.ComponentModel.DataAnnotations;

namespace EggsAndHoney.WebApi.ViewModels
{
    public class AddOrderViewModel
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Order { get; set; }
    }
}
