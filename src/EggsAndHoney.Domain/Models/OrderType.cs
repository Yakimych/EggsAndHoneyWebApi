using System.ComponentModel.DataAnnotations;

namespace EggsAndHoney.Domain.Models
{
    public class OrderType
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
