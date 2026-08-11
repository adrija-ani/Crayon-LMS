using Crayon.Entity.Common;
using Crayon.Entity.Data;
using Crayon.Entity.Dto;
using Crayon.Services.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace Crayon.Services.Repository.Implementation
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly ApplicationDbContext _context;

        public AttendanceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<AttendanceResponse>> CheckIn(AttendanceRequest request)
        {
            Result<AttendanceResponse> result = new();

            try
            {
                var employee = await _context.EmployeeSet
                    .FirstOrDefaultAsync(x => x.UserId == request.UserId);

                if (employee == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Employee Not Found"
                    });

                    return result;
                }

                bool alreadyCheckedIn = await _context.AttendanceSet.AnyAsync(x =>
                    x.EmployeeId == employee.EmployeeId &&
                    x.AttendanceDate.Date == DateTime.Today);

                if (alreadyCheckedIn)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 400,
                        ErrorMessage = "Already Checked In Today"
                    });

                    return result;
                }

                Attendance attendance = new Attendance
                {
                    EmployeeId = employee.EmployeeId,
                    AttendanceDate = DateTime.Today,
                    CheckInTime = DateTime.Now.TimeOfDay,
                    AttendanceStatus = "Present",
                    HoursWorked = 0,
                    IsApproved = false
                };

                _context.AttendanceSet.Add(attendance);

                await _context.SaveChangesAsync();

                result.Response = new AttendanceResponse
                {
                    AttendanceId = attendance.AttendanceId,
                    EmployeeId = employee.EmployeeId,
                    EmployeeName = employee.FirstName + " " + employee.LastName,
                    AttendanceDate = attendance.AttendanceDate,
                    CheckInTime = attendance.CheckInTime,
                    AttendanceStatus = attendance.AttendanceStatus,
                    HoursWorked = attendance.HoursWorked,
                    IsApproved = attendance.IsApproved
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
        public async Task<Result<bool>> CheckOut(string userId)
        {
            Result<bool> result = new();

            try
            {
                var employee = await _context.EmployeeSet
                    .FirstOrDefaultAsync(x => x.UserId == userId);

                if (employee == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Employee Not Found"
                    });

                    return result;
                }

                var attendance = await _context.AttendanceSet
                    .FirstOrDefaultAsync(x =>
                        x.EmployeeId == employee.EmployeeId &&
                        x.AttendanceDate.Date == DateTime.Today);

                if (attendance == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Check In Record Not Found"
                    });

                    return result;
                }

                if (attendance.CheckOutTime != null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 400,
                        ErrorMessage = "Already Checked Out"
                    });

                    return result;
                }

                attendance.CheckOutTime = DateTime.Now.TimeOfDay;

                attendance.HoursWorked =
                    Convert.ToDecimal(
                        (attendance.CheckOutTime.Value -
                         attendance.CheckInTime).TotalHours);

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

        public async Task<Result<List<AttendanceResponse>>> GetAllAttendance()
        {
            Result<List<AttendanceResponse>> result = new();
            try
            {
                var attendances = await (
                    from a in _context.AttendanceSet

                    join e in _context.EmployeeSet
                        on a.EmployeeId equals e.EmployeeId

                    select new AttendanceResponse
                    {
                        AttendanceId = a.AttendanceId,
                        EmployeeId = e.EmployeeId,
                        EmployeeName = e.FirstName + " " + e.LastName,
                        AttendanceDate = a.AttendanceDate,
                        CheckInTime = a.CheckInTime,
                        CheckOutTime = a.CheckOutTime,
                        HoursWorked = a.HoursWorked,
                        AttendanceStatus = a.AttendanceStatus,
                        IsApproved = a.IsApproved
                    } ).ToListAsync();
                result.Response = attendances;

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

        public async Task<Result<AttendanceResponse>> GetAttendanceById(int attendanceId)
        {
            Result<AttendanceResponse> result = new();
            try
            {
                var attendance = await (
                    from a in _context.AttendanceSet

                    join e in _context.EmployeeSet
                        on a.EmployeeId equals e.EmployeeId

                    where a.AttendanceId == attendanceId

                    select new AttendanceResponse
                    {
                        AttendanceId = a.AttendanceId,
                        EmployeeId = e.EmployeeId,
                        EmployeeName = e.FirstName + " " + e.LastName,
                        AttendanceDate = a.AttendanceDate,
                        CheckInTime = a.CheckInTime,
                        CheckOutTime = a.CheckOutTime,
                        HoursWorked = a.HoursWorked,
                        AttendanceStatus = a.AttendanceStatus,
                        IsApproved = a.IsApproved
                    }).FirstOrDefaultAsync();
                if (attendance == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Attendance Not Found"
                    });
                    return result;
                }
                result.Response = attendance;
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


        public async Task<Result<List<AttendanceResponse>>> GetEmployeeAttendance(string userId)
        {
            Result<List<AttendanceResponse>> result = new();

            try
            {
                var employee = await _context.EmployeeSet
                    .FirstOrDefaultAsync(x => x.UserId == userId);

                if (employee == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Employee Not Found"
                    });

                    return result;
                }

                var attendances = await (
                    from a in _context.AttendanceSet

                    join e in _context.EmployeeSet
                        on a.EmployeeId equals e.EmployeeId

                    where a.EmployeeId == employee.EmployeeId

                    orderby a.AttendanceDate descending

                    select new AttendanceResponse
                    {
                        AttendanceId = a.AttendanceId,
                        EmployeeId = e.EmployeeId,
                        EmployeeName = e.FirstName + " " + e.LastName,
                        AttendanceDate = a.AttendanceDate,
                        CheckInTime = a.CheckInTime,
                        CheckOutTime = a.CheckOutTime,
                        HoursWorked = a.HoursWorked,
                        AttendanceStatus = a.AttendanceStatus,
                        IsApproved = a.IsApproved
                    }).ToListAsync();

                result.Response = attendances;

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
