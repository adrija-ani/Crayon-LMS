using Crayon.Entity.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Attendance")]
public class Attendance
{
    [Key]
    public int AttendanceId { get; set; }

    [Required]
    public int EmployeeId { get; set; }

    public DateTime AttendanceDate { get; set; }

    public TimeSpan CheckInTime { get; set; }

    public TimeSpan? CheckOutTime { get; set; }

    public decimal HoursWorked { get; set; }

    public string AttendanceStatus { get; set; }

    public bool IsApproved { get; set; }

    [ForeignKey(nameof(EmployeeId))]
    public Employee Employee { get; set; }
}