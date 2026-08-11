using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crayon.Entity.Models
{
    [Table("AttendanceLog")]
    public class AttendanceLog
    {
        [Key]
        public int AttendanceLogId { get; set; }

        public int AttendanceId { get; set; }

        [Required]
        public string Action { get; set; }

        public DateTime ActionDate { get; set; }

        [ForeignKey(nameof(AttendanceId))]
        public Attendance Attendance { get; set; }
    }
}