using System;
using System.Collections.Generic;
using System.Text;

namespace Crayon.Entity.Dto
{
    public class HolidayRequest
    {
        public string HolidayName { get; set; }

        public DateTime HolidayDate { get; set; }

        public string Country { get; set; }
    }
}
