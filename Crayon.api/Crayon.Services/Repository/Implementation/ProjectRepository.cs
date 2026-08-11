using Crayon.Entity.Common;
using Crayon.Entity.Data;
using Crayon.Entity.Dto;
using Crayon.Entity.Models;
using Crayon.Services.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace Crayon.Services.Repository.Implementation
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result<ProjectResponse>> AddProject(ProjectRequest request)
        {
            Result<ProjectResponse> result = new();
            try
            {
                bool exists = await _context.ProjectSet.AnyAsync(x => x.ProjectName == request.ProjectName && x.IsActive);
                if (exists)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 400,
                        ErrorMessage = "Project Already Exists"
                    });
                    return result;
                }
                Project project = new()
                {
                    ProjectName = request.ProjectName,
                    Description = request.Description,
                    IsActive = true
                };

                _context.ProjectSet.Add(project);

                await _context.SaveChangesAsync();

                result.Response = new ProjectResponse
                {
                    ProjectId = project.ProjectId,
                    ProjectName = project.ProjectName,
                    Description = project.Description
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

        public async Task<Result<bool>> DeleteProject(int projectId)
        {
            Result<bool> result = new();
            try
            {
                var project = await _context.ProjectSet
                    .FirstOrDefaultAsync(x => x.ProjectId == projectId && x.IsActive);
                if (project == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Project Not Found"
                    });

                    return result;
                }
                project.IsActive = false;
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

        public async Task<Result<List<ProjectResponse>>> GetAllProjects()
        {
            Result<List<ProjectResponse>> result = new();

            try
            {
                var projects = await _context.ProjectSet
                    .Where(x => x.IsActive)
                    .Select(x => new ProjectResponse
                    {
                        ProjectId = x.ProjectId,
                        ProjectName = x.ProjectName,
                        Description = x.Description
                    }).ToListAsync();
                result.Response = projects;
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

        public async Task<Result<ProjectResponse>> GetProjectById(int projectId)
        {
            Result<ProjectResponse> result = new();
            try
            {
                var project = await _context.ProjectSet
                    .Where(x =>
                        x.ProjectId == projectId &&
                        x.IsActive)
                    .Select(x => new ProjectResponse
                    {
                        ProjectId = x.ProjectId,
                        ProjectName = x.ProjectName,
                        Description = x.Description
                    }).FirstOrDefaultAsync();
                if (project == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Project Not Found"
                    });
                    return result;
                }
                result.Response = project;
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

        public async Task<Result<bool>> UpdateProject(int projectId,ProjectRequest request)
        {
            Result<bool> result = new();
            try
            {
                var project = await _context.ProjectSet
                    .FirstOrDefaultAsync(x =>
                        x.ProjectId == projectId &&
                        x.IsActive);

                if (project == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Project Not Found"
                    });

                    return result;
                }
                bool exists = await _context.ProjectSet
                    .AnyAsync(x =>
                        x.ProjectName == request.ProjectName &&
                        x.ProjectId != projectId &&
                        x.IsActive);
                if (exists)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 400,
                        ErrorMessage = "Project Already Exists"
                    });

                    return result;
                }
                project.ProjectName = request.ProjectName;
                project.Description = request.Description;
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
