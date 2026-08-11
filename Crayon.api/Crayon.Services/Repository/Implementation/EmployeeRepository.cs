using Crayon.Entity.Common;
using Crayon.Entity.Data;
using Crayon.Entity.Dto;
using Microsoft.EntityFrameworkCore;
using Crayon.Services.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Crayon.Services.Repository.Implementation
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<EmployeeResponse>>> GetAllEmployees()
        {
            Result<List<EmployeeResponse>> result = new();

            try
            {
                var employees = await (

                    from e in _context.EmployeeSet

                    join d in _context.DepartmentSet
                        on e.DepartmentId equals d.DepartmentId

                    join des in _context.DesignationSet
                        on e.DesignationId equals des.DesignationId

                    join w in _context.WorkplaceSet
                        on e.WorkplaceId equals w.WorkplaceId

                    join u in _context.Users
                        on e.UserId equals u.Id

                    join rm in _context.EmployeeSet
                        on e.ReportingEmployeeId equals rm.EmployeeId
                        into managerGroup

                    from manager in managerGroup.DefaultIfEmpty()

                    where e.IsActive

                    select new EmployeeResponse
                    {
                        EmployeeId   = e.EmployeeId,
                        UserId       = e.UserId,
                        EmployeeCode = e.EmployeeCode,
                        FirstName    = e.FirstName,
                        LastName     = e.LastName,
                        FullName     = e.FirstName + " " + e.LastName,
                        Email        = u.Email,
                        PhoneNumber  = e.PhoneNumber,

                        DepartmentId   = d.DepartmentId,
                        DepartmentName = d.DepartmentName,

                        DesignationId   = des.DesignationId,
                        DesignationName = des.DesignationName,

                        WorkplaceId   = w.WorkplaceId,
                        WorkplaceName = w.Name,

                        RoleName = (
                            from ur in _context.UserRoles
                            join r  in _context.Roles on ur.RoleId equals r.Id
                            where ur.UserId == u.Id
                            select r.Name
                        ).FirstOrDefault(),

                        ReportingManagerId = e.ReportingEmployeeId,

                        ReportingManagerName =
                            manager != null
                            ? manager.FirstName + " " + manager.LastName
                            : "",

                        IsActive    = e.IsActive,
                        CreatedDate = e.CreatedDate
                    }

                ).ToListAsync();

                result.Response = employees;

                return result;
            }
            catch (Exception ex)
            {
                result.Error.Add(new Errors
                {
                    ErrorCode    = 500,
                    ErrorMessage = ex.Message
                });

                return result;
            }
        }

        public async Task<Result<bool>> DeleteEmployee(int employeeId)
        {
            Result<bool> result = new();

            try
            {
                var employee = await _context.EmployeeSet
                    .FirstOrDefaultAsync(x => x.EmployeeId == employeeId && x.IsActive);

                if (employee == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode    = 404,
                        ErrorMessage = "Employee Not Found"
                    });

                    return result;
                }

                employee.IsActive = false;

                await _context.SaveChangesAsync();

                result.Response = true;

                return result;
            }
            catch (Exception ex)
            {
                result.Error.Add(new Errors
                {
                    ErrorCode    = 500,
                    ErrorMessage = ex.Message
                });

                return result;
            }
        }

        public async Task<Result<EmployeeResponse>> GetEmployeeById(int employeeId)
        {
            Result<EmployeeResponse> result = new();

            try
            {
                var employee = await (

                    from e in _context.EmployeeSet

                    join d in _context.DepartmentSet
                        on e.DepartmentId equals d.DepartmentId

                    join des in _context.DesignationSet
                        on e.DesignationId equals des.DesignationId

                    join w in _context.WorkplaceSet
                        on e.WorkplaceId equals w.WorkplaceId

                    join u in _context.Users
                        on e.UserId equals u.Id

                    join rm in _context.EmployeeSet
                        on e.ReportingEmployeeId equals rm.EmployeeId
                        into managerGroup

                    from manager in managerGroup.DefaultIfEmpty()

                    where e.EmployeeId == employeeId

                    select new EmployeeResponse
                    {
                        EmployeeId   = e.EmployeeId,
                        UserId       = e.UserId,
                        EmployeeCode = e.EmployeeCode,
                        FirstName    = e.FirstName,
                        LastName     = e.LastName,
                        FullName     = e.FirstName + " " + e.LastName,
                        Email        = u.Email,
                        PhoneNumber  = e.PhoneNumber,

                        DepartmentId   = d.DepartmentId,
                        DepartmentName = d.DepartmentName,

                        DesignationId   = des.DesignationId,
                        DesignationName = des.DesignationName,

                        WorkplaceId   = w.WorkplaceId,
                        WorkplaceName = w.Name,

                        RoleName = (
                            from ur in _context.UserRoles
                            join r  in _context.Roles on ur.RoleId equals r.Id
                            where ur.UserId == u.Id
                            select r.Name
                        ).FirstOrDefault(),

                        ReportingManagerId = e.ReportingEmployeeId,

                        ReportingManagerName =
                            manager != null
                            ? manager.FirstName + " " + manager.LastName
                            : "",

                        IsActive    = e.IsActive,
                        CreatedDate = e.CreatedDate
                    }

                ).FirstOrDefaultAsync();

                if (employee == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode    = 404,
                        ErrorMessage = "Employee Not Found"
                    });

                    return result;
                }

                result.Response = employee;

                return result;
            }
            catch (Exception ex)
            {
                result.Error.Add(new Errors
                {
                    ErrorCode    = 500,
                    ErrorMessage = ex.Message
                });

                return result;
            }
        }

        public async Task<Result<List<EmployeeResponse>>> GetEmployeesByDepartment(int departmentId)
        {
            Result<List<EmployeeResponse>> result = new();

            try
            {
                var employees = await (

                    from e in _context.EmployeeSet

                    join d in _context.DepartmentSet
                        on e.DepartmentId equals d.DepartmentId

                    join des in _context.DesignationSet
                        on e.DesignationId equals des.DesignationId

                    where e.DepartmentId == departmentId
                          && e.IsActive

                    select new EmployeeResponse
                    {
                        EmployeeId      = e.EmployeeId,
                        EmployeeCode    = e.EmployeeCode,
                        FirstName       = e.FirstName,
                        LastName        = e.LastName,
                        DepartmentName  = d.DepartmentName,
                        DesignationName = des.DesignationName,
                        PhoneNumber     = e.PhoneNumber
                    }

                ).ToListAsync();

                result.Response = employees;

                return result;
            }
            catch (Exception ex)
            {
                result.Error.Add(new Errors
                {
                    ErrorCode    = 500,
                    ErrorMessage = ex.Message
                });

                return result;
            }
        }

        public async Task<Result<bool>> UpdateEmployee(
            int employeeId,
            EmployeeRequest request)
        {
            Result<bool> result = new();

            try
            {
                var employee = await _context.EmployeeSet
                    .FirstOrDefaultAsync(x => x.EmployeeId == employeeId);

                if (employee == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode    = 404,
                        ErrorMessage = "Employee Not Found"
                    });

                    return result;
                }

                employee.FirstName         = request.FirstName;
                employee.LastName          = request.LastName;
                employee.PhoneNumber       = request.PhoneNumber;
                employee.DepartmentId      = request.DepartmentId;
                employee.DesignationId     = request.DesignationId;
                employee.WorkplaceId       = request.WorkplaceId;
                employee.EmployeeCode      = request.EmployeeCode;
                employee.ReportingEmployeeId = request.ReportingManagerId;

                await _context.SaveChangesAsync();

                result.Response = true;

                return result;
            }
            catch (Exception ex)
            {
                result.Error.Add(new Errors
                {
                    ErrorCode    = 500,
                    ErrorMessage = ex.Message
                });

                return result;
            }
        }

        public async Task<Result<List<EmployeeResponse>>> GetMyTeam(string userId)
        {
            Result<List<EmployeeResponse>> result = new();

            try
            {
                var manager = await _context.EmployeeSet
                    .FirstOrDefaultAsync(x => x.UserId == userId);

                if (manager == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode    = 404,
                        ErrorMessage = "Manager record not found"
                    });

                    return result;
                }

                var employees = await (

                    from e in _context.EmployeeSet

                    join d in _context.DepartmentSet
                        on e.DepartmentId equals d.DepartmentId

                    join des in _context.DesignationSet
                        on e.DesignationId equals des.DesignationId

                    join u in _context.Users
                        on e.UserId equals u.Id

                    where e.ReportingEmployeeId == manager.EmployeeId
                          && e.IsActive

                    select new EmployeeResponse
                    {
                        EmployeeId      = e.EmployeeId,
                        UserId          = e.UserId,
                        EmployeeCode    = e.EmployeeCode,
                        FirstName       = e.FirstName,
                        LastName        = e.LastName,
                        FullName        = e.FirstName + " " + e.LastName,
                        Email           = u.Email,
                        PhoneNumber     = e.PhoneNumber,
                        DepartmentName  = d.DepartmentName,
                        DesignationName = des.DesignationName,
                        IsActive        = e.IsActive
                    }

                ).ToListAsync();

                result.Response = employees;

                return result;
            }
            catch (Exception ex)
            {
                result.Error.Add(new Errors
                {
                    ErrorCode    = 500,
                    ErrorMessage = ex.Message
                });

                return result;
            }
        }
    }
}
