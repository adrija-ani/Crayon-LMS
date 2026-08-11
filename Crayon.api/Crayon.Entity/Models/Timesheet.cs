using Crayon.Entity.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Timesheet")]
public class Timesheet
{
    [Key]
    public int TimesheetId { get; set; }

    public int EmployeeId { get; set; }

    public int ProjectId { get; set; }

    public int ProjectTaskId { get; set; }

    public decimal HoursWorked { get; set; }

    public DateTime WorkDate { get; set; }

    [Required]
    public string WorkDescription { get; set; }

    [ForeignKey(nameof(EmployeeId))]
    public Employee Employee { get; set; }

    [ForeignKey(nameof(ProjectId))]
    public Project Project { get; set; }

    [ForeignKey(nameof(ProjectTaskId))]
    public ProjectTask ProjectTask { get; set; }
}