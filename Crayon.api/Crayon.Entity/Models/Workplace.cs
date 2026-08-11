using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crayon.Entity.Models
{
    [Table("Workplace")]
    public class Workplace
    {
        [Key]
        public int WorkplaceId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }
        public bool IsActive { get; set; } = true;

    }
}