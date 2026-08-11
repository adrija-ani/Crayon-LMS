using System;
using System.Collections.Generic;
using System.Text;

namespace Crayon.Entity.Dto
{
    public class HolidayResponse
    {
        public int HolidayId { get; set; }

        public string HolidayName { get; set; }

        public DateTime HolidayDate { get; set; }

        public string Country { get; set; }
    }
}
