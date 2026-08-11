using System;
using System.Collections.Generic;
using System.Text;

namespace Crayon.Entity.Dto
{
    public class LeaveBalanceResponse
    {
        public int LeaveBalanceId { get; set; }

        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public int LeaveTypeId { get; set; }

        public string LeaveName { get; set; }

        public decimal AvailableDays { get; set; }
    }
}
