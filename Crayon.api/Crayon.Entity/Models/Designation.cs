using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crayon.Entity.Models
{
    [Table("Designation")]
    public class Designation
    {
        [Key]
        public int DesignationId { get; set; }

        [Required]
        [StringLength(100)]
        public string DesignationName { get; set; }
        
        public bool IsActive { get; set; }

    }
}