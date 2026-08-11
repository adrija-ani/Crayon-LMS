using System;
using System.Collections.Generic;
using System.Text;

namespace Crayon.Entity.Dto
{
    public class AttendanceResponse
    {
        public int AttendanceId { get; set; }

        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public DateTime AttendanceDate { get; set; }

        public TimeSpan CheckInTime { get; set; }

        public TimeSpan? CheckOutTime { get; set; }

        public decimal HoursWorked { get; set; }

        public string AttendanceStatus { get; set; }

        public bool IsApproved { get; set; }
    }
}
