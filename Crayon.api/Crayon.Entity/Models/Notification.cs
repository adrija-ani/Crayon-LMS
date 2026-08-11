using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crayon.Entity.Models
{
    [Table("Notification")]
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

        public string UserId { get; set; }

        public string Message { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedDate { get; set; }

        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; }
    }
}