using System;
using System.Collections.Generic;
using System.Text;

namespace Crayon.Services.GenericHttpClient
{
    public class ApiConstant
    {
        #region
        public const string Login = "/api/Employee/Login";

        public const string RegisterEmployee = "/api/Employee/RegisterEmployee";

        public const string GetAllEmployee = "/api/Employee/GetAllEmployees";

        public const string GetEmployeeById = "/api/Employee/GetEmployeeById/";

        public const string UpdateEmployee = "/api/Employee/UpdateEmployee/";

        public const string DeleteEmployee = "/api/Employee/DeleteEmployee/";

        public const string GetEmployeesByDepartment = "/api/Employee/GetEmployeesByDepartment/";

        public const string GetMyTeam   = "/api/Employee/GetMyTeam";

        public const string GetAllRoles = "/api/Employee/GetAllRoles";
        #endregion


        #region
        public const string AddDepartment = "/api/Department/AddDepartment";

        public const string GetAllDepartments = "/api/Department/GetAllDepartments";

        public const string GetDepartmentById = "/api/Department/GetDepartmentById/";

        public const string UpdateDepartment = "/api/Department/UpdateDepartment/";
        #endregion

        #region
        public const string AddDesignation = "api/Designation/AddDesignation";

        public const string GetAllDesignations = "api/Designation/GetAllDesignations";

        public const string GetDesignationById = "api/Designation/GetDesignationById";

        public const string UpdateDesignation = "api/Designation/UpdateDesignation";

        public const string DeleteDesignation = "api/Designation/DeleteDesignation";
        #endregion

        #region
        public const string AddWorkplace = "api/Workplace/AddWorkplace";

        public const string GetAllWorkplaces = "api/Workplace/GetAllWorkplaces";

        public const string GetWorkplaceById = "api/Workplace/GetWorkplaceById";

        public const string UpdateWorkplace = "api/Workplace/UpdateWorkplace";

        public const string DeleteWorkplace = "api/Workplace/DeleteWorkplace";
        #endregion

        #region
        public const string AddHoliday = "api/Holiday/AddHoliday";

        public const string GetAllHolidays = "api/Holiday/GetAllHolidays";

        public const string GetHolidayById = "api/Holiday/GetHolidayById";

        public const string UpdateHoliday = "api/Holiday/UpdateHoliday";

        public const string DeleteHoliday = "api/Holiday/DeleteHoliday";
        #endregion


        #region
        public const string GetAllLeaveTypes = "api/LeaveType/GetAllLeaveTypes";

        public const string GetLeaveTypeById = "api/LeaveType/GetLeaveTypeById";

        public const string AddLeaveType = "api/LeaveType/AddLeaveType";

        public const string UpdateLeaveType = "api/LeaveType/UpdateLeaveType";

        public const string DeleteLeaveType = "api/LeaveType/DeleteLeaveType";
        #endregion

        #region
        public const string ApplyLeave = "api/LeaveApplication/ApplyLeave";

        public const string GetAllLeaveApplications = "api/LeaveApplication/GetAllLeaveApplications";

        public const string GetLeaveApplicationById = "api/LeaveApplication/GetLeaveApplicationById";

        public const string GetEmployeeLeaveApplications = "api/LeaveApplication/GetEmployeeLeaveApplications";

        public const string ApproveLeave = "api/LeaveApplication/ApproveLeave";

        public const string RejectLeave = "api/LeaveApplication/RejectLeave";

        public const string CancelLeave = "api/LeaveApplication/CancelLeave";

        public const string GetManagerLeaveApplications = "api/LeaveApplication/GetManagerLeaveApplications";
        #endregion

        #region
        public const string CheckIn = "api/Attendance/CheckIn";

        public const string CheckOut = "api/Attendance/CheckOut";

        public const string GetEmployeeAttendance = "api/Attendance/GetEmployeeAttendance";

        public const string GetAllAttendance = "api/Attendance/GetAllAttendance";

        public const string GetAttendanceById = "api/Attendance/GetAttendanceById";
        #endregion

        #region
        public const string AddLeaveBalance = "api/LeaveBalance/AddLeaveBalance";

        public const string GetEmployeeLeaveBalances = "api/LeaveBalance/GetEmployeeLeaveBalances";

        public const string GetLeaveBalanceById = "api/LeaveBalance/GetLeaveBalanceById";

        public const string UpdateLeaveBalance = "api/LeaveBalance/UpdateLeaveBalance";

        public const string DeleteLeaveBalance = "api/LeaveBalance/DeleteLeaveBalance";

        public const string GetAllLeaveBalances = "api/LeaveBalance/GetAllLeaveBalances";
        #endregion

        #region
        public const string AddTimesheet = "api/Timesheet/AddTimesheet";

        public const string GetAllTimesheets = "api/Timesheet/GetAllTimesheets";

        public const string GetTimesheetById = "api/Timesheet/GetTimesheetById";

        public const string GetEmployeeTimesheets = "api/Timesheet/GetEmployeeTimesheets";

        public const string UpdateTimesheet = "api/Timesheet/UpdateTimesheet";

        public const string DeleteTimesheet = "api/Timesheet/DeleteTimesheet";
        #endregion

        #region
        public const string AddProject = "api/Project/AddProject";

        public const string GetAllProjects = "api/Project/GetAllProjects";

        public const string GetProjectById = "api/Project/GetProjectById";

        public const string UpdateProject = "api/Project/UpdateProject";

        public const string DeleteProject = "api/Project/DeleteProject";
        #endregion

        #region
        public const string AddProjectTask = "api/ProjectTask/AddProjectTask";

        public const string GetAllProjectTasks = "api/ProjectTask/GetAllProjectTasks";

        public const string GetProjectTaskById = "api/ProjectTask/GetProjectTaskById";

        public const string GetTasksByProject = "api/ProjectTask/GetTasksByProject";

        public const string UpdateProjectTask = "api/ProjectTask/UpdateProjectTask";

        public const string DeleteProjectTask = "api/ProjectTask/DeleteProjectTask";
        #endregion

        #region
        public const string GetEmployeeCount    = "/api/Dashboard/GetEmployeeCount";

        public const string GetDepartmentCount  = "/api/Dashboard/GetDepartmentCount";

        public const string GetProjectCount     = "/api/Dashboard/GetProjectCount";

        public const string GetPendingLeaveCount = "/api/Dashboard/GetPendingLeaveCount";
        #endregion

        #region
        public const string GetVisibleEmployees = "api/Employee/GetVisibleEmployees";

        #endregion




    }
}
