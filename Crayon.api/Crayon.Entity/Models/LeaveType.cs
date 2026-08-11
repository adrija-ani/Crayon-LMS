using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crayon.Entity.Models
{
    [Table("LeaveType")]
    public class LeaveType
    {
        [Key]
        public int LeaveTypeId { get; set; }

        [Required]
        public string LeaveName { get; set; }

        public int DefaultDays { get; set; }
        public bool IsActive { get; set; } = true;

    }
}