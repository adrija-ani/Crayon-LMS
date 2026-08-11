using Crayon.Entity.Common;
using Crayon.Entity.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Crayon.Services.Repository.Interface
{
    public interface IAttendanceRepository
    {
        Task<Result<AttendanceResponse>> CheckIn(AttendanceRequest request);
        Task<Result<bool>> CheckOut(string userId);
        Task<Result<List<AttendanceResponse>>> GetAllAttendance();
        Task<Result<AttendanceResponse>> GetAttendanceById(int attendanceId);
        Task<Result<List<AttendanceResponse>>> GetEmployeeAttendance(string userId);
    }
}
