using System;
using System.Collections.Generic;
using System.Text;
using System;

namespace Crayon.Entity.Dto
{
    public class EmployeeResponse
    {
        public int EmployeeId { get; set; }

        public string UserId { get; set; }

        public string EmployeeCode { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public int DesignationId { get; set; }

        public string DesignationName { get; set; }

        public int WorkplaceId { get; set; }

        public string WorkplaceName { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? ReportingManagerId { get; set; }

        public string ReportingManagerName { get; set; }
        public string RoleName { get; set; }
    }
}