using System;
using System.Collections.Generic;
using System.Text;

namespace Crayon.Entity.Dto
{
    public class LeaveApplicationResponse
    {
        public int LeaveApplicationId { get; set; }
        public string EmployeeName { get; set; }
        public string LeaveName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int TotalDays { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public DateTime AppliedDate { get; set; }
    }
}
