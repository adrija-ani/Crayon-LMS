using System;
using System.Collections.Generic;
using System.Text;

namespace Crayon.Entity.Dto
{
    public class EmployeeRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmployeeCode { get; set; }

        public string PhoneNumber { get; set; }

        public int DepartmentId { get; set; }

        public int DesignationId { get; set; }

        public int WorkplaceId { get; set; }

        public int? ReportingManagerId { get; set; }

        public string RoleName { get; set; }
    }
}
