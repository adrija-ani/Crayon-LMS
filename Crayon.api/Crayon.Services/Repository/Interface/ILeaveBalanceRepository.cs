using Crayon.Entity.Common;
using Crayon.Entity.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Crayon.Services.Repository.Interface
{
    public interface ILeaveBalanceRepository
    {
        Task<Result<LeaveBalanceResponse>> AddLeaveBalance(LeaveBalanceRequest request);
        Task<Result<List<LeaveBalanceResponse>>> GetAllLeaveBalances();
        Task<Result<LeaveBalanceResponse>> GetLeaveBalanceById(int leaveBalanceId);
        Task<Result<bool>> UpdateLeaveBalance(int leaveBalanceId,LeaveBalanceRequest request);
        Task<Result<bool>> DeleteLeaveBalance(int leaveBalanceId);

        Task<Result<List<LeaveBalanceResponse>>> GetEmployeeLeaveBalances(string userId);
    }
}
