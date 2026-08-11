using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crayon.Entity.Models
{
    [Table("Holiday")]
    public class Holiday
    {
        [Key]
        public int HolidayId { get; set; }

        public string HolidayName { get; set; }

        public DateTime HolidayDate { get; set; }

        public string Country { get; set; }
    }
}