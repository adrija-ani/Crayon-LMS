using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crayon.Entity.Models
{
    [Table("ProjectTask")]
    public class ProjectTask
    {
        [Key]
        public int ProjectTaskId { get; set; }

        public int ProjectId { get; set; }

        public string TaskName { get; set; }

        [ForeignKey(nameof(ProjectId))]
        public Project Project { get; set; }
    }
}