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
    public class DesignationRepository : IDesignationRepository
    {
        private readonly ApplicationDbContext _context;

        public DesignationRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result<DesignationResponse>> AddDesignation(DesignationRequest request)
        {
            Result<DesignationResponse> result = new();
            try
            {
                bool exists = await _context.DesignationSet.AnyAsync(x => x.DesignationName == request.DesignationName && x.IsActive);
                if (exists)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 400,
                        ErrorMessage = "Designation Already Exists"
                    });
                    return result;
                }
                Designation designation = new()
                {
                    DesignationName = request.DesignationName,
                    IsActive = true
                };
                _context.DesignationSet.Add(designation);
                await _context.SaveChangesAsync();
                result.Response = new DesignationResponse
                {
                    DesignationId = designation.DesignationId,
                    DesignationName = designation.DesignationName
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

        public async Task<Result<bool>> DeleteDesignation(int designationId)
        {
            Result<bool> result = new();
            try
            {
                var designation = await _context.DesignationSet
                    .FirstOrDefaultAsync(x =>
                        x.DesignationId == designationId &&
                        x.IsActive);
                if (designation == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Designation Not Found"
                    });
                    return result;
                }
                designation.IsActive = false;
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

        public async Task<Result<List<DesignationResponse>>> GetAllDesignations()
        {
            Result<List<DesignationResponse>> result = new();
            try
            {
                var designations = await _context.DesignationSet
                    .Where(x => x.IsActive)
                    .Select(x => new DesignationResponse
                    {
                        DesignationId = x.DesignationId,
                        DesignationName = x.DesignationName
                    })
                    .ToListAsync();
                result.Response = designations;
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

        public async Task<Result<DesignationResponse>> GetDesignationById(int designationId)
        {
            Result<DesignationResponse> result = new();
            try
            {
                var designation = await _context.DesignationSet
                    .Where(x => x.DesignationId == designationId && x.IsActive)
                    .Select(x => new DesignationResponse
                    {
                        DesignationId = x.DesignationId,
                        DesignationName = x.DesignationName
                        
                    })
                    .FirstOrDefaultAsync();
                if (designation == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Designation Not Found"
                    });
                    return result;
                }
                result.Response = designation;
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

        public async Task<Result<bool>> UpdateDesignation(int designationId,DesignationRequest request)
        {
            Result<bool> result = new();
            try
            {
                var designation = await _context.DesignationSet
                    .FirstOrDefaultAsync(x =>
                        x.DesignationId == designationId &&
                        x.IsActive);
                if (designation == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Designation Not Found"
                    });
                    return result;
                }
                bool exists = await _context.DesignationSet
                    .AnyAsync(x =>
                        x.DesignationName == request.DesignationName &&
                        x.DesignationId != designationId &&
                        x.IsActive);
                if (exists)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 400,
                        ErrorMessage = "Designation Already Exists"
                    });

                    return result;
                }
                designation.DesignationName = request.DesignationName;
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
