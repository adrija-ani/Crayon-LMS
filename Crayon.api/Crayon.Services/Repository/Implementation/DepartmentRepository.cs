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
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;

        public DepartmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result<DepartmentResponse>> AddDepartment(DepartmentRequest request)
        {
            Result<DepartmentResponse> result = new();
            try
            {
                bool exists = await _context.DepartmentSet.AnyAsync(x => x.DepartmentName == request.DepartmentName);
                if (exists)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 400,
                        ErrorMessage = "Department Already Exists"
                    });

                    return result;
                }
                Department department = new()
                {
                    DepartmentName = request.DepartmentName
                    
                };
                _context.DepartmentSet.Add(department);
                await _context.SaveChangesAsync();
                result.Response = new DepartmentResponse
                {
                    DepartmentId = department.DepartmentId,
                    DepartmentName = department.DepartmentName
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

        public async Task<Result<bool>> DeleteDepartment(int departmentId)
        {
            Result<bool> result = new();
            try
            {
                var department = await _context.DepartmentSet.FirstOrDefaultAsync(d => d.DepartmentId == departmentId);

                if (department == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Department Not Found"
                    });
                    return result;
                }
                department.IsActive = false;
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
        public async Task<Result<List<DepartmentResponse>>> GetAllDepartments()
        {
            Result<List<DepartmentResponse>> result = new();
            try
            {
                var departments = await _context.DepartmentSet
                    .Where(d => d.IsActive)
                    .Select(d => new DepartmentResponse
                    {
                        DepartmentId = d.DepartmentId,
                        DepartmentName = d.DepartmentName
                    })
                    .ToListAsync();
                result.Response = departments;
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
        public async Task<Result<DepartmentResponse>> GetDepartmentById(int departmentId)
        {
            Result<DepartmentResponse> result = new();
            try
            {
                var department = await _context.DepartmentSet
                    .Where(d => d.DepartmentId == departmentId && d.IsActive)
                    .Select(d => new DepartmentResponse
                    {
                        DepartmentId = d.DepartmentId,
                        DepartmentName = d.DepartmentName
                    })
                    .FirstOrDefaultAsync();
                if (department == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Department Not Found"
                    });

                    return result;
                }
                result.Response = department;
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

        public async Task<Result<bool>> UpdateDepartment(int departmentId,DepartmentRequest request)
        {
            Result<bool> result = new();
            try
            {
                var department = await _context.DepartmentSet.FirstOrDefaultAsync(d => d.DepartmentId == departmentId);
                if (department == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Department Not Found"
                    });
                    return result;
                }
                bool exists = await _context.DepartmentSet
                    .AnyAsync(d =>
                        d.DepartmentName == request.DepartmentName &&
                        d.DepartmentId != departmentId);
                if (exists)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 400,
                        ErrorMessage = "Department Already Exists"
                    });
                    return result;
                }

                department.DepartmentName = request.DepartmentName;
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
