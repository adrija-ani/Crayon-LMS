using System;
using System.Collections.Generic;
using System.Text;

namespace Crayon.Entity.Dto
{
    public class LeaveBalanceRequest
    {
        public int EmployeeId { get; set; }

        public int LeaveTypeId { get; set; }

        public decimal AvailableDays { get; set; }
    }
}
