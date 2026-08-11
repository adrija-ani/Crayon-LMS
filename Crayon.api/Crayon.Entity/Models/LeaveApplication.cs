using Crayon.Entity.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("LeaveApplication")]
public class LeaveApplication
{
    [Key]
    public int LeaveApplicationId { get; set; }

    public int EmployeeId { get; set; }

    public int LeaveTypeId { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public int TotalDays { get; set; }

    public string Reason { get; set; }

    public string Status { get; set; }

    public DateTime AppliedDate { get; set; }

    [ForeignKey(nameof(EmployeeId))]
    public Employee Employee { get; set; }

    [ForeignKey(nameof(LeaveTypeId))]
    public LeaveType LeaveType { get; set; }
}