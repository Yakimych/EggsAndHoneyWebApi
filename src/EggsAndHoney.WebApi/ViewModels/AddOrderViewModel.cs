using System.ComponentModel.DataAnnotations;

namespace EggsAndHoney.WebApi.ViewModels
{
    public class AddOrderViewModel
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }
        
        [Required, MaxLength(50)]
        public string Order { get; set; }
    }
}
