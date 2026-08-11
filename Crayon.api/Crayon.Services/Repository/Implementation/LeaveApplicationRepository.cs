using Crayon.Entity.Common;
using Crayon.Entity.Data;
using Crayon.Entity.Dto;
using Crayon.Entity.Models;
using Crayon.Services.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace Crayon.Services.Repository.Implementation
{
    public class LeaveApplicationRepository : ILeaveApplicationRepository
    {
        private readonly ApplicationDbContext _context;

        public LeaveApplicationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<LeaveApplicationResponse>> ApplyLeave(LeaveApplicationRequest request)
        {
            Result<LeaveApplicationResponse> result = new();

            try
            {
                var employee = await _context.EmployeeSet
                    .FirstOrDefaultAsync(x =>
                        x.UserId == request.UserId);

                if (employee == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Employee Not Found"
                    });

                    return result;
                }

                var leaveType = await _context.LeaveTypeSet
                    .FirstOrDefaultAsync(x =>
                        x.LeaveTypeId == request.LeaveTypeId);

                if (leaveType == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Leave Type Not Found"
                    });

                    return result;
                }

                if (request.FromDate.Date < DateTime.Today)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 400,
                        ErrorMessage = "Cannot apply leave for past dates"
                    });

                    return result;
                }

                if (request.ToDate < request.FromDate)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 400,
                        ErrorMessage = "To Date cannot be less than From Date"
                    });

                    return result;
                }

                int totalDays =
                    (request.ToDate.Date -
                     request.FromDate.Date).Days + 1;

                var leaveBalance = await _context.LeaveBalanceSet
                    .FirstOrDefaultAsync(x =>
                        x.EmployeeId == employee.EmployeeId &&
                        x.LeaveTypeId == request.LeaveTypeId);


                if (leaveBalance.AvailableDays < totalDays)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 400,
                        ErrorMessage =
                            $"Insufficient Leave Balance. Available Days : {leaveBalance.AvailableDays}"
                    });

                    return result;
                }

                bool overlapExists = await _context.LeaveApplicationSet
                    .AnyAsync(x =>
                        x.EmployeeId == employee.EmployeeId &&
                        x.Status != LeaveStatus.Cancelled &&
                        request.FromDate <= x.ToDate &&
                        request.ToDate >= x.FromDate);

                if (overlapExists)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 400,
                        ErrorMessage =
                            "Leave already exists for selected dates"
                    });

                    return result;
                }

                LeaveApplication leaveApplication = new()
                {
                    EmployeeId = employee.EmployeeId,
                    LeaveTypeId = request.LeaveTypeId,
                    FromDate = request.FromDate,
                    ToDate = request.ToDate,
                    TotalDays = totalDays,
                    Reason = request.Reason,
                    Status = LeaveStatus.Pending,
                    AppliedDate = DateTime.Now
                };

                _context.LeaveApplicationSet.Add(
                    leaveApplication);

                await _context.SaveChangesAsync();

                result.Response = new LeaveApplicationResponse
                {
                    LeaveApplicationId =
                        leaveApplication.LeaveApplicationId,

                    EmployeeName =
                        employee.FirstName + " " +
                        employee.LastName,

                    LeaveName = leaveType.LeaveName,

                    FromDate = leaveApplication.FromDate,
                    ToDate = leaveApplication.ToDate,

                    TotalDays = leaveApplication.TotalDays,

                    Reason = leaveApplication.Reason,

                    Status = leaveApplication.Status,

                    AppliedDate = leaveApplication.AppliedDate
                };

                return result;
            }
            catch (Exception ex)
            {
                result.Error.Add(new Errors
                {
                    ErrorCode = 500,
                    ErrorMessage = ex.Message
                });

                return result;
            }
        }
        public async Task<Result<bool>> ApproveLeave(int leaveApplicationId)
        {
            Result<bool> result = new();

            try
            {
                var leaveApplication = await _context.LeaveApplicationSet
                    .FirstOrDefaultAsync(x =>
                        x.LeaveApplicationId == leaveApplicationId);

                if (leaveApplication == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Leave Application Not Found"
                    });

                    return result;
                }

                if (leaveApplication.Status == LeaveStatus.Approved)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 400,
                        ErrorMessage = "Leave Already Approved"
                    });

                    return result;
                }

                if (leaveApplication.Status == LeaveStatus.Rejected)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 400,
                        ErrorMessage = "Rejected Leave Cannot Be Approved"
                    });

                    return result;
                }

                var leaveBalance = await _context.LeaveBalanceSet
                    .FirstOrDefaultAsync(x =>
                        x.EmployeeId == leaveApplication.EmployeeId &&
                        x.LeaveTypeId == leaveApplication.LeaveTypeId);

                if (leaveBalance == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Leave Balance Not Found"
                    });

                    return result;
                }

                if (leaveBalance.AvailableDays < leaveApplication.TotalDays)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 400,
                        ErrorMessage = "Insufficient Leave Balance"
                    });

                    return result;
                }

                leaveBalance.AvailableDays -= leaveApplication.TotalDays;

                leaveApplication.Status = LeaveStatus.Approved;

                await _context.SaveChangesAsync();

                result.Response = true;

                return result;
            }
            catch (Exception ex)
            {
                result.Error.Add(new Errors
                {
                    ErrorCode = 500,
                    ErrorMessage = ex.Message
                });

                return result;
            }
        }

        public async Task<Result<bool>> CancelLeave(int leaveApplicationId)
        {
            Result<bool> result = new();
            try
            {
                var leaveApplication = await _context.LeaveApplicationSet.FirstOrDefaultAsync(x => x.LeaveApplicationId == leaveApplicationId);
                if (leaveApplication == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Leave Application Not Found"
                    });
                    return result;
                }
                if (leaveApplication.Status != LeaveStatus.Pending)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 400,
                        ErrorMessage = "Only Pending Leave Can Be Cancelled"
                    });
                    return result;
                }
                leaveApplication.Status = LeaveStatus.Cancelled;
                await _context.SaveChangesAsync();
                result.Response = true;
                return result;
            }
            catch (Exception ex)
            {
                result.Error.Add(new Errors
                {
                    ErrorCode = 500,
                    ErrorMessage = ex.Message
                });
                return result;
            }
        }

        public async Task<Result<List<LeaveApplicationResponse>>> GetAllLeaveApplications()
        {
            Result<List<LeaveApplicationResponse>> result = new();
            try
            {
                var leaveApplications = await (
                    from la in _context.LeaveApplicationSet

                    join e in _context.EmployeeSet
                        on la.EmployeeId equals e.EmployeeId

                    join lt in _context.LeaveTypeSet
                        on la.LeaveTypeId equals lt.LeaveTypeId

                    select new LeaveApplicationResponse
                    {
                        LeaveApplicationId = la.LeaveApplicationId,
                        EmployeeName = e.FirstName + " " + e.LastName,
                        LeaveName = lt.LeaveName,
                        FromDate = la.FromDate,
                        ToDate = la.ToDate,
                        TotalDays = la.TotalDays,
                        Reason = la.Reason,
                        Status = la.Status,
                        AppliedDate = la.AppliedDate
                    }
                ).ToListAsync();
                result.Response = leaveApplications;
                return result;
            }
            catch (Exception ex)
            {
                result.Error.Add(new Errors
                {
                    ErrorCode = 500,
                    ErrorMessage = ex.Message
                });
                return result;
            }
        }

        public async Task<Result<List<LeaveApplicationResponse>>> GetEmployeeLeaveApplications(string userId)
        {
            Result<List<LeaveApplicationResponse>> result = new();

            try
            {
                var leaveApplications = await (
                    from la in _context.LeaveApplicationSet

                    join e in _context.EmployeeSet
                        on la.EmployeeId equals e.EmployeeId

                    join lt in _context.LeaveTypeSet
                        on la.LeaveTypeId equals lt.LeaveTypeId

                    where e.UserId == userId

                    select new LeaveApplicationResponse
                    {
                        LeaveApplicationId = la.LeaveApplicationId,
                        EmployeeName = e.FirstName + " " + e.LastName,
                        LeaveName = lt.LeaveName,
                        FromDate = la.FromDate,
                        ToDate = la.ToDate,
                        TotalDays = la.TotalDays,
                        Reason = la.Reason,
                        Status = la.Status,
                        AppliedDate = la.AppliedDate
                    }).ToListAsync();

                result.Response = leaveApplications;

                return result;
            }
            catch (Exception ex)
            {
                result.Error.Add(new Errors
                {
                    ErrorCode = 500,
                    ErrorMessage = ex.Message
                });

                return result;
            }
        }

        public async Task<Result<LeaveApplicationResponse>> GetLeaveApplicationById(int leaveApplicationId)
        {
            Result<LeaveApplicationResponse> result = new();
            try
            {
                var leaveApplication = await (
                    from la in _context.LeaveApplicationSet

                    join e in _context.EmployeeSet
                        on la.EmployeeId equals e.EmployeeId

                    join lt in _context.LeaveTypeSet
                        on la.LeaveTypeId equals lt.LeaveTypeId

                    where la.LeaveApplicationId == leaveApplicationId

                    select new LeaveApplicationResponse
                    {
                        LeaveApplicationId = la.LeaveApplicationId,
                        EmployeeName = e.FirstName + " " + e.LastName,
                        LeaveName = lt.LeaveName,
                        FromDate = la.FromDate,
                        ToDate = la.ToDate,
                        TotalDays = la.TotalDays,
                        Reason = la.Reason,
                        Status = la.Status,
                        AppliedDate = la.AppliedDate
                    }).FirstOrDefaultAsync();
                if (leaveApplication == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Leave Application Not Found"
                    });
                    return result;
                }
                result.Response = leaveApplication;
                return result;
            }
            catch (Exception ex)
            {
                result.Error.Add(new Errors
                {
                    ErrorCode = 500,
                    ErrorMessage = ex.Message
                });
                return result;
            }
        }

        public async Task<Result<bool>> RejectLeave(int leaveApplicationId)
        {
            Result<bool> result = new();
            try
            {
                var leaveApplication = await _context.LeaveApplicationSet.FirstOrDefaultAsync(x => x.LeaveApplicationId == leaveApplicationId);

                if (leaveApplication == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Leave Application Not Found"
                    });

                    return result;
                }
                leaveApplication.Status = LeaveStatus.Rejected;
                await _context.SaveChangesAsync();
                result.Response = true;
                return result;
            }
            catch (Exception ex)
            {
                result.Error.Add(new Errors
                {
                    ErrorCode = 500,
                    ErrorMessage = ex.Message
                });
                return result;
            }
        }

        public async Task<Result<List<LeaveApplicationResponse>>> GetManagerLeaveApplications(string managerUserId)
        {
            Result<List<LeaveApplicationResponse>> result = new();
            try
            {
                var manager = await _context.EmployeeSet
                    .FirstOrDefaultAsync(e => e.UserId == managerUserId);

                if (manager == null)
                {
                    result.Response = new List<LeaveApplicationResponse>();
                    return result;
                }

                var applications = await (
                    from la in _context.LeaveApplicationSet
                    join e  in _context.EmployeeSet   on la.EmployeeId  equals e.EmployeeId
                    join lt in _context.LeaveTypeSet  on la.LeaveTypeId equals lt.LeaveTypeId
                    where e.ReportingEmployeeId == manager.EmployeeId
                    select new LeaveApplicationResponse
                    {
                        LeaveApplicationId = la.LeaveApplicationId,
                        EmployeeName       = e.FirstName + " " + e.LastName,
                        LeaveName          = lt.LeaveName,
                        FromDate           = la.FromDate,
                        ToDate             = la.ToDate,
                        TotalDays          = la.TotalDays,
                        Reason             = la.Reason,
                        Status             = la.Status,
                        AppliedDate        = la.AppliedDate
                    }
                ).ToListAsync();

                result.Response = applications;
                return result;
            }
            catch (Exception ex)
            {
                result.Error.Add(new Errors { ErrorCode = 500, ErrorMessage = ex.Message });
                return result;
            }
        }
    }
}
