using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crayon.Entity.Models
{
    [Table("ApprovalWorkflow")]
    public class ApprovalWorkflow
    {
        [Key]
        public int ApprovalWorkflowId { get; set; }

        public int LeaveApplicationId { get; set; }

        public string ApproverId { get; set; }

        public string Status { get; set; }

        public DateTime ActionDate { get; set; }

        [ForeignKey(nameof(LeaveApplicationId))]
        public LeaveApplication LeaveApplication { get; set; }
    }
}