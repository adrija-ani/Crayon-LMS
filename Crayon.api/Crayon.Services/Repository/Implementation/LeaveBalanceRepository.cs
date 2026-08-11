using Crayon.Entity.Common;
using Crayon.Entity.Data;
using Crayon.Entity.Dto;
using Crayon.Entity.Models;
using Microsoft.EntityFrameworkCore;
using Crayon.Services.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Crayon.Services.Repository.Implementation
{
    public class LeaveBalanceRepository : ILeaveBalanceRepository
    {
        private readonly ApplicationDbContext _context;

        public LeaveBalanceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<LeaveBalanceResponse>> AddLeaveBalance(LeaveBalanceRequest request)
        {
            Result<LeaveBalanceResponse> result = new();
            try
            {
                bool employeeExists = await _context.EmployeeSet.AnyAsync(x => x.EmployeeId == request.EmployeeId);
                if (!employeeExists)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Employee Not Found"
                    });
                    return result;
                }
                bool leaveTypeExists = await _context.LeaveTypeSet.AnyAsync(x => x.LeaveTypeId == request.LeaveTypeId);
                if (!leaveTypeExists)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Leave Type Not Found"
                    });

                    return result;
                }
                bool exists = await _context.LeaveBalanceSet.AnyAsync(x =>
                        x.EmployeeId == request.EmployeeId &&
                        x.LeaveTypeId == request.LeaveTypeId);
                if (exists)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 400,
                        ErrorMessage = "Leave Balance Already Exists"
                    });

                    return result;
                }

                LeaveBalance leaveBalance = new()
                {
                    EmployeeId = request.EmployeeId,
                    LeaveTypeId = request.LeaveTypeId,
                    AvailableDays = request.AvailableDays
                };
                _context.LeaveBalanceSet.Add(leaveBalance);
                await _context.SaveChangesAsync();
                result.Response = new LeaveBalanceResponse
                {
                    LeaveBalanceId = leaveBalance.LeaveBalanceId,
                    EmployeeId = leaveBalance.EmployeeId,
                    LeaveTypeId = leaveBalance.LeaveTypeId,
                    AvailableDays = leaveBalance.AvailableDays
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

        public async Task<Result<bool>> DeleteLeaveBalance(int leaveBalanceId)
        {
            Result<bool> result = new();
            try
            {
                var leaveBalance = await _context.LeaveBalanceSet.FirstOrDefaultAsync(x => x.LeaveBalanceId == leaveBalanceId);
                if (leaveBalance == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Leave Balance Not Found"
                    });
                    return result;
                }
                _context.LeaveBalanceSet.Remove(leaveBalance);
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

        public async Task<Result<List<LeaveBalanceResponse>>> GetAllLeaveBalances()
        {
            Result<List<LeaveBalanceResponse>> result = new();
            try
            {
                var leaveBalances = await (
                    from lb in _context.LeaveBalanceSet

                    join e in _context.EmployeeSet
                        on lb.EmployeeId equals e.EmployeeId

                    join lt in _context.LeaveTypeSet
                        on lb.LeaveTypeId equals lt.LeaveTypeId

                    select new LeaveBalanceResponse
                    {
                        LeaveBalanceId = lb.LeaveBalanceId,
                        EmployeeId = e.EmployeeId,
                        EmployeeName = e.FirstName + " " + e.LastName,
                        LeaveTypeId = lt.LeaveTypeId,
                        LeaveName = lt.LeaveName,
                        AvailableDays = lb.AvailableDays
                    }
                ).ToListAsync();
                result.Response = leaveBalances;
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

        public async Task<Result<LeaveBalanceResponse>> GetLeaveBalanceById(int leaveBalanceId)
        {
            Result<LeaveBalanceResponse> result = new();
            try
            {
                var leaveBalance = await (
                    from lb in _context.LeaveBalanceSet

                    join e in _context.EmployeeSet
                        on lb.EmployeeId equals e.EmployeeId

                    join lt in _context.LeaveTypeSet
                        on lb.LeaveTypeId equals lt.LeaveTypeId

                    where lb.LeaveBalanceId == leaveBalanceId
                    select new LeaveBalanceResponse
                    {
                        LeaveBalanceId = lb.LeaveBalanceId,
                        EmployeeId = e.EmployeeId,
                        EmployeeName = e.FirstName + " " + e.LastName,
                        LeaveTypeId = lt.LeaveTypeId,
                        LeaveName = lt.LeaveName,
                        AvailableDays = lb.AvailableDays
                    }
                ).FirstOrDefaultAsync();
                if (leaveBalance == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Leave Balance Not Found"
                    });
                    return result;
                }
                result.Response = leaveBalance;
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

        public async Task<Result<bool>> UpdateLeaveBalance(int leaveBalanceId,LeaveBalanceRequest request)
        {
            Result<bool> result = new();
            try
            {
                var leaveBalance = await _context.LeaveBalanceSet.FirstOrDefaultAsync(x => x.LeaveBalanceId == leaveBalanceId);
                if (leaveBalance == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Leave Balance Not Found"
                    });
                    return result;
                }
                bool exists = await _context.LeaveBalanceSet
                    .AnyAsync(x =>
                        x.EmployeeId == request.EmployeeId &&
                        x.LeaveTypeId == request.LeaveTypeId &&
                        x.LeaveBalanceId != leaveBalanceId);
                if (exists)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 400,
                        ErrorMessage = "Leave Balance Already Exists"
                    });
                    return result;
                }
                leaveBalance.EmployeeId = request.EmployeeId;
                leaveBalance.LeaveTypeId = request.LeaveTypeId;
                leaveBalance.AvailableDays = request.AvailableDays;
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

        public async Task<Result<List<LeaveBalanceResponse>>>GetEmployeeLeaveBalances(string userId)
        {
            Result<List<LeaveBalanceResponse>> result = new();

            try
            {
                var balances = await (
                    from lb in _context.LeaveBalanceSet

                    join e in _context.EmployeeSet
                        on lb.EmployeeId equals e.EmployeeId

                    join lt in _context.LeaveTypeSet
                        on lb.LeaveTypeId equals lt.LeaveTypeId

                    where e.UserId == userId

                    select new LeaveBalanceResponse
                    {
                        LeaveBalanceId = lb.LeaveBalanceId,
                        EmployeeId     = e.EmployeeId,
                        EmployeeName   = e.FirstName + " " + e.LastName,
                        LeaveTypeId    = lt.LeaveTypeId,
                        LeaveName      = lt.LeaveName,
                        AvailableDays  = lb.AvailableDays
                    }
                ).ToListAsync();

                result.Response = balances;

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

    }
}
