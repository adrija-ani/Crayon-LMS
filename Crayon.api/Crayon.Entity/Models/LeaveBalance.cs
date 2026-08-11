using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crayon.Entity.Models
{
    [Table("LeaveBalance")]
    public class LeaveBalance
    {
        [Key]
        public int LeaveBalanceId { get; set; }

        public int EmployeeId { get; set; }

        public int LeaveTypeId { get; set; }

        public decimal AvailableDays { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; }

        [ForeignKey(nameof(LeaveTypeId))]
        public LeaveType LeaveType { get; set; }
    }
}