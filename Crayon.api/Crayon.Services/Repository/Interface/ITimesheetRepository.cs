using Crayon.Entity.Common;
using Crayon.Entity.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Crayon.Services.Repository.Interface
{
    public interface ITimesheetRepository
    {
        Task<Result<TimesheetResponse>> AddTimesheet(TimesheetRequest request);
        Task<Result<List<TimesheetResponse>>> GetAllTimesheets();
        Task<Result<TimesheetResponse>> GetTimesheetById(int timesheetId);

        Task<Result<List<TimesheetResponse>>> GetEmployeeTimesheets(string userId);
        Task<Result<bool>> UpdateTimesheet(int timesheetId,TimesheetRequest request);

        Task<Result<bool>> DeleteTimesheet(int timesheetId);
    }
}