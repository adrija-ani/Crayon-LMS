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
    public class LeaveTypeRepository : ILeaveTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public LeaveTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result<LeaveTypeResponse>> AddLeaveType(LeaveTypeRequest request)
        {
            Result<LeaveTypeResponse> result = new();
            try
            {
                bool exists = await _context.LeaveTypeSet
                    .AnyAsync(x =>
                        x.LeaveName == request.LeaveName &&
                        x.IsActive);

                if (exists)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 400,
                        ErrorMessage = "Leave Type Already Exists"
                    });

                    return result;
                }
                LeaveType leaveType = new()
                {
                    LeaveName = request.LeaveName,
                    DefaultDays = request.DefaultDays,
                    IsActive = true
                };
                _context.LeaveTypeSet.Add(leaveType);
                await _context.SaveChangesAsync();
                result.Response = new LeaveTypeResponse
                {
                    LeaveTypeId = leaveType.LeaveTypeId,
                    LeaveName = leaveType.LeaveName,
                    DefaultDays = leaveType.DefaultDays
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

        public async Task<Result<bool>> DeleteLeaveType(int leaveTypeId)
        {
            Result<bool> result = new();
            try
            {
                var leaveType = await _context.LeaveTypeSet
                    .FirstOrDefaultAsync(x =>
                        x.LeaveTypeId == leaveTypeId &&
                        x.IsActive);
                if (leaveType == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Leave Type Not Found"
                    });
                    return result;
                }
                leaveType.IsActive = false;
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

        public async Task<Result<List<LeaveTypeResponse>>> GetAllLeaveTypes()
        {
            Result<List<LeaveTypeResponse>> result = new();
            try
            {
                var leaveTypes = await _context.LeaveTypeSet
                    .Where(x => x.IsActive)
                    .Select(x => new LeaveTypeResponse
                    {
                        LeaveTypeId = x.LeaveTypeId,
                        LeaveName = x.LeaveName,
                        DefaultDays = x.DefaultDays
                    }).ToListAsync();
                result.Response = leaveTypes;
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

        public async Task<Result<LeaveTypeResponse>> GetLeaveTypeById(int leaveTypeId)
        {
            Result<LeaveTypeResponse> result = new();
            try
            {
                var leaveType = await _context.LeaveTypeSet
                    .Where(x => x.LeaveTypeId == leaveTypeId && x.IsActive)
                    .Select(x => new LeaveTypeResponse
                    {
                        LeaveTypeId = x.LeaveTypeId,
                        LeaveName = x.LeaveName,
                        DefaultDays = x.DefaultDays
                    }).FirstOrDefaultAsync();
                if (leaveType == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Leave Type Not Found"
                    });
                    return result;
                }
                result.Response = leaveType;
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

        public async Task<Result<bool>> UpdateLeaveType(int leaveTypeId,LeaveTypeRequest request)
        {
            Result<bool> result = new();
            try
            {
                var leaveType = await _context.LeaveTypeSet
                    .FirstOrDefaultAsync(x =>
                        x.LeaveTypeId == leaveTypeId &&
                        x.IsActive);
                if (leaveType == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Leave Type Not Found"
                    });
                    return result;
                }
                bool exists = await _context.LeaveTypeSet
                    .AnyAsync(x =>
                        x.LeaveName == request.LeaveName &&
                        x.LeaveTypeId != leaveTypeId &&
                        x.IsActive);
                if (exists)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 400,
                        ErrorMessage = "Leave Type Already Exists"
                    });
                    return result;
                }
                leaveType.LeaveName = request.LeaveName;
                leaveType.DefaultDays = request.DefaultDays;
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
    }
}
