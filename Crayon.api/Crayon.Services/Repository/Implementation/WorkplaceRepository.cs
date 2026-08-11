using Crayon.Entity.Common;
using Crayon.Entity.Data;
using Crayon.Entity.Dto;
using Crayon.Entity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Crayon.Services.Repository.Implementation
{
    public class WorkplaceRepository : IWorkplaceRepository
    {
        private readonly ApplicationDbContext _context;

        public WorkplaceRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result<WorkplaceResponse>> AddWorkplace(WorkplaceRequest request)
        {
            Result<WorkplaceResponse> result = new();
            try
            {
                bool exists = await _context.WorkplaceSet.AnyAsync(x => x.Name == request.Name && x.IsActive);
                if (exists)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 400,
                        ErrorMessage = "Workplace Already Exists"
                    });

                    return result;
                }
                Workplace workplace = new()
                {
                    Name = request.Name,
                    Address = request.Address,
                    IsActive = true
                };
                _context.WorkplaceSet.Add(workplace);
                await _context.SaveChangesAsync();
                result.Response = new WorkplaceResponse
                {
                    WorkplaceId = workplace.WorkplaceId,
                    Name = workplace.Name,
                    Address = workplace.Address
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

        public async Task<Result<bool>> DeleteWorkplace(int workplaceId)
        {
            Result<bool> result = new();
            try
            {
                var workplace = await _context.WorkplaceSet
                    .FirstOrDefaultAsync(x =>
                        x.WorkplaceId == workplaceId &&
                        x.IsActive);
                if (workplace == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Workplace Not Found"
                    });

                    return result;
                }
                workplace.IsActive = false;
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

        public async Task<Result<List<WorkplaceResponse>>> GetAllWorkplaces()
        {
            Result<List<WorkplaceResponse>> result = new();
            try
            {
                var workplaces = await _context.WorkplaceSet
                    .Where(x => x.IsActive)
                    .Select(x => new WorkplaceResponse
                    {
                        WorkplaceId = x.WorkplaceId,
                        Name = x.Name,
                        Address = x.Address
                    })
                    .ToListAsync();
                result.Response = workplaces;
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

        public async Task<Result<WorkplaceResponse>> GetWorkplaceById(int workplaceId)
        {
            Result<WorkplaceResponse> result = new();
            try
            {
                var workplace = await _context.WorkplaceSet
                    .Where(x => x.WorkplaceId == workplaceId &&
                                x.IsActive)
                    .Select(x => new WorkplaceResponse
                    {
                        WorkplaceId = x.WorkplaceId,
                        Name = x.Name,
                        Address = x.Address
                    })
                    .FirstOrDefaultAsync();
                if (workplace == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Workplace Not Found"
                    });
                    return result;
                }
                result.Response = workplace;
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

        public async Task<Result<bool>> UpdateWorkplace(int workplaceId,WorkplaceRequest request)
        {
            Result<bool> result = new();
            try
            {
                var workplace = await _context.WorkplaceSet
                    .FirstOrDefaultAsync(x =>
                        x.WorkplaceId == workplaceId &&
                        x.IsActive);
                if (workplace == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Workplace Not Found"
                    });

                    return result;
                }
                bool exists = await _context.WorkplaceSet
                    .AnyAsync(x =>
                        x.Name == request.Name &&
                        x.WorkplaceId != workplaceId &&
                        x.IsActive);
                if (exists)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 400,
                        ErrorMessage = "Workplace Already Exists"
                    });

                    return result;
                }
                workplace.Name = request.Name;
                workplace.Address = request.Address;
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
