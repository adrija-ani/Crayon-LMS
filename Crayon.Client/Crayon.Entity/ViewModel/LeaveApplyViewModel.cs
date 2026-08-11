using Crayon.Entity.Dto;

namespace Crayon.Client.Models
{
    public class LeaveApplyViewModel
    {
        public LeaveApplicationRequest LeaveApplication { get; set; }

        public List<LeaveBalanceResponse> LeaveBalances { get; set; } = new();
    }
}