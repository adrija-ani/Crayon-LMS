using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crayon.Entity.Models
{
    [Table("Department")]
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required]
        [StringLength(100)]
        public string DepartmentName { get; set; }
        public bool IsActive { get; set; } = true;
    }
}