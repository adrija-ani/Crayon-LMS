using Crayon.Entity.Common;
using Crayon.Entity.Data;
using Crayon.Entity.Dto;
using Crayon.Entity.Models;
using Crayon.Services.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Crayon.Services.Repository.Implementation
{
    public class TimesheetRepository : ITimesheetRepository
    {
        private readonly ApplicationDbContext _context;

        public TimesheetRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<TimesheetResponse>> AddTimesheet(TimesheetRequest request)
        {
            Result<TimesheetResponse> result = new();

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

                var project = await _context.ProjectSet
                    .FirstOrDefaultAsync(x =>
                        x.ProjectId == request.ProjectId);

                if (project == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Project Not Found"
                    });

                    return result;
                }

                var task = await _context.ProjectTaskSet
                    .FirstOrDefaultAsync(x =>
                        x.ProjectTaskId == request.ProjectTaskId);

                if (task == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Project Task Not Found"
                    });

                    return result;
                }

                Timesheet timesheet = new()
                {
                    EmployeeId = employee.EmployeeId,
                    ProjectId = request.ProjectId,
                    ProjectTaskId = request.ProjectTaskId,
                    HoursWorked = request.HoursWorked,
                    WorkDate = request.WorkDate,
                    WorkDescription = request.WorkDescription
                };

                _context.TimesheetSet.Add(timesheet);

                await _context.SaveChangesAsync();

                result.Response = new TimesheetResponse
                {
                    TimesheetId = timesheet.TimesheetId,
                    EmployeeId = employee.EmployeeId,
                    EmployeeName = employee.FirstName + " " + employee.LastName,
                    ProjectId = project.ProjectId,
                    ProjectName = project.ProjectName,
                    ProjectTaskId = task.ProjectTaskId,
                    TaskName = task.TaskName,
                    HoursWorked = timesheet.HoursWorked,
                    WorkDate = timesheet.WorkDate,
                    WorkDescription = timesheet.WorkDescription
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

        public async Task<Result<bool>> DeleteTimesheet(int timesheetId)
        {
            Result<bool> result = new();
            try
            {
                var timesheet = await _context.TimesheetSet
                    .FirstOrDefaultAsync(x => x.TimesheetId == timesheetId);
                if (timesheet == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Timesheet Not Found"
                    });
                    return result;
                }
                _context.TimesheetSet.Remove(timesheet);
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

        public async Task<Result<List<TimesheetResponse>>> GetAllTimesheets()
        {
            Result<List<TimesheetResponse>> result = new();

            try
            {
                var timesheets = await (
                    from t in _context.TimesheetSet

                    join e in _context.EmployeeSet
                        on t.EmployeeId equals e.EmployeeId

                    join p in _context.ProjectSet
                        on t.ProjectId equals p.ProjectId

                    join pt in _context.ProjectTaskSet
                        on t.ProjectTaskId equals pt.ProjectTaskId

                    select new TimesheetResponse
                    {
                        TimesheetId = t.TimesheetId,
                        EmployeeId = e.EmployeeId,
                        EmployeeName = e.FirstName + " " + e.LastName,

                        ProjectId = p.ProjectId,
                        ProjectName = p.ProjectName,

                        ProjectTaskId = pt.ProjectTaskId,
                        TaskName = pt.TaskName,

                        HoursWorked = t.HoursWorked,
                        WorkDate = t.WorkDate,
                        WorkDescription = t.WorkDescription
                    }
                ).ToListAsync();

                result.Response = timesheets;

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


        public async Task<Result<List<TimesheetResponse>>> GetEmployeeTimesheets(string userId)
        {
            Result<List<TimesheetResponse>> result = new();

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

                var timesheets = await (
                    from t in _context.TimesheetSet

                    join e in _context.EmployeeSet
                        on t.EmployeeId equals e.EmployeeId

                    join p in _context.ProjectSet
                        on t.ProjectId equals p.ProjectId

                    join pt in _context.ProjectTaskSet
                        on t.ProjectTaskId equals pt.ProjectTaskId

                    where t.EmployeeId == employee.EmployeeId

                    select new TimesheetResponse
                    {
                        TimesheetId = t.TimesheetId,

                        EmployeeId = e.EmployeeId,
                        EmployeeName = e.FirstName + " " + e.LastName,

                        ProjectId = p.ProjectId,
                        ProjectName = p.ProjectName,

                        ProjectTaskId = pt.ProjectTaskId,
                        TaskName = pt.TaskName,

                        HoursWorked = t.HoursWorked,
                        WorkDate = t.WorkDate,
                        WorkDescription = t.WorkDescription
                    }
                ).ToListAsync();

                result.Response = timesheets;

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

        public async Task<Result<TimesheetResponse>> GetTimesheetById(int timesheetId)
        {
            Result<TimesheetResponse> result = new();

            try
            {
                var timesheet = await (
                    from t in _context.TimesheetSet

                    join e in _context.EmployeeSet
                        on t.EmployeeId equals e.EmployeeId

                    join p in _context.ProjectSet
                        on t.ProjectId equals p.ProjectId

                    join pt in _context.ProjectTaskSet
                        on t.ProjectTaskId equals pt.ProjectTaskId

                    where t.TimesheetId == timesheetId

                    select new TimesheetResponse
                    {
                        TimesheetId = t.TimesheetId,
                        EmployeeId = e.EmployeeId,
                        EmployeeName = e.FirstName + " " + e.LastName,

                        ProjectId = p.ProjectId,
                        ProjectName = p.ProjectName,

                        ProjectTaskId = pt.ProjectTaskId,
                        TaskName = pt.TaskName,

                        HoursWorked = t.HoursWorked,
                        WorkDate = t.WorkDate,
                        WorkDescription = t.WorkDescription
                    }
                ).FirstOrDefaultAsync();

                if (timesheet == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Timesheet Not Found"
                    });

                    return result;
                }

                result.Response = timesheet;

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
        public async Task<Result<bool>> UpdateTimesheet(int timesheetId,TimesheetRequest request)
        {
            Result<bool> result = new();

            try
            {
                var timesheet = await _context.TimesheetSet
                    .FirstOrDefaultAsync(x =>
                        x.TimesheetId == timesheetId);

                if (timesheet == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Timesheet Not Found"
                    });

                    return result;
                }


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

                timesheet.EmployeeId = employee.EmployeeId;
                timesheet.ProjectId = request.ProjectId;
                timesheet.ProjectTaskId = request.ProjectTaskId;
                timesheet.HoursWorked = request.HoursWorked;
                timesheet.WorkDate = request.WorkDate;
                timesheet.WorkDescription = request.WorkDescription;

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
 

