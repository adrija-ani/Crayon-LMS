using System.ComponentModel.DataAnnotations;

namespace Crayon.Entity.Dto
{
    public class TimesheetRequest
    {
        public string UserId { get; set; }

        public int ProjectId { get; set; }

        public int ProjectTaskId { get; set; }

        public decimal HoursWorked { get; set; }

        public DateTime WorkDate { get; set; }

        [Required(ErrorMessage = "Today's Progress is required")]
        public string WorkDescription { get; set; }
    }
}