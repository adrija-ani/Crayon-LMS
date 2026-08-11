using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crayon.Entity.Models
{
    [Table("Project")]
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }

        [Required]
        public string ProjectName { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }
    }
}