using System;
using System.Collections.Generic;
using System.Text;

namespace Crayon.Entity.Dto
{
    public class TimesheetResponse
    {
        public int TimesheetId { get; set; }

        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public int ProjectId { get; set; }

        public string ProjectName { get; set; }

        public int ProjectTaskId { get; set; }

        public string TaskName { get; set; }

        public decimal HoursWorked { get; set; }

        public DateTime WorkDate { get; set; }
        public string WorkDescription { get; set; }
    }
}
