using Crayon.Entity.Common;
using Crayon.Entity.Dto;

namespace Crayon.Services.Repository.Interface
{
    public interface ILeaveApplicationRepository
    {
        Task<Result<LeaveApplicationResponse>> ApplyLeave(LeaveApplicationRequest request);
        Task<Result<List<LeaveApplicationResponse>>> GetAllLeaveApplications();
        Task<Result<LeaveApplicationResponse>> GetLeaveApplicationById(int leaveApplicationId);
        Task<Result<List<LeaveApplicationResponse>>> GetEmployeeLeaveApplications(string employeeId);
        Task<Result<bool>> ApproveLeave(int leaveApplicationId);
        Task<Result<bool>> RejectLeave(int leaveApplicationId);
        Task<Result<bool>> CancelLeave(int leaveApplicationId);
        Task<Result<List<LeaveApplicationResponse>>> GetManagerLeaveApplications(string managerUserId);
    }
}