using Crayon.Entity.Common;
using Crayon.Entity.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Crayon.Services.Repository.Interface
{
    public interface IDepartmentRepository
    {
        Task<Result<DepartmentResponse>> AddDepartment(DepartmentRequest request);
        Task<Result<List<DepartmentResponse>>> GetAllDepartments();
        Task<Result<DepartmentResponse>> GetDepartmentById(int departmentId);
        Task<Result<bool>> UpdateDepartment(int departmentId,DepartmentRequest request);
        Task<Result<bool>> DeleteDepartment(int departmentId);

    }
}
