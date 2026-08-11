using Crayon.Entity.Common;
using Crayon.Entity.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Crayon.Services.Repository.Interface
{
    public interface IEmployeeRepository
    {
        Task<Result<List<EmployeeResponse>>> GetAllEmployees();
        Task<Result<EmployeeResponse>> GetEmployeeById(int employeeId);
        Task<Result<bool>> UpdateEmployee(int employeeId,EmployeeRequest request);
        Task<Result<bool>> DeleteEmployee(int employeeId);
        Task<Result<List<EmployeeResponse>>> GetEmployeesByDepartment(int departmentId);
        Task<Result<List<EmployeeResponse>>> GetMyTeam(string userId);
    }
}
