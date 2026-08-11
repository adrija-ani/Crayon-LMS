using Crayon.Entity.Common;
using Crayon.Entity.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Crayon.Services.Repository.Interface
{
    public interface ILeaveTypeRepository
    {
        Task<Result<LeaveTypeResponse>> AddLeaveType(LeaveTypeRequest request);

        Task<Result<List<LeaveTypeResponse>>> GetAllLeaveTypes();

        Task<Result<LeaveTypeResponse>> GetLeaveTypeById(int leaveTypeId);

        Task<Result<bool>> UpdateLeaveType(int leaveTypeId, LeaveTypeRequest request);

        Task<Result<bool>> DeleteLeaveType(int leaveTypeId);
    }
}
