using System;
using System.Collections.Generic;
using System.Text;

namespace Crayon.Entity.Dto
{
    public class LeaveTypeRequest
    {
        public string LeaveName { get; set; }

        public int DefaultDays { get; set; }
    }
}
