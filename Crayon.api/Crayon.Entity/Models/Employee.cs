using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;


namespace Crayon.Entity.Models
{
    [Table("Employee")]
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public int DesignationId { get; set; }

        [Required]
        public int WorkplaceId { get; set; }

        [StringLength(50)]
        public string EmployeeCode { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        [ForeignKey(nameof(UserId))]

        public virtual IdentityUser User { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public virtual Department Department { get; set; }

        [ForeignKey(nameof(DesignationId))]
        public virtual Designation Designation { get; set; }

        [ForeignKey(nameof(WorkplaceId))]
        public virtual Workplace Workplace { get; set; }

        public int? ReportingEmployeeId { get; set; }

        [ForeignKey(nameof(ReportingEmployeeId))]
        public virtual Employee ReportingEmployee { get; set; }

        public int? ReportingToEmployeeId { get; set; }

        [ForeignKey(nameof(ReportingToEmployeeId))]
        public Employee? ReportingToEmployee { get; set; }

        public ICollection<Employee>? Subordinates { get; set; }
    }
}