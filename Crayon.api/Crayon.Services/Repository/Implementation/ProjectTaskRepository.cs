using Crayon.Entity.Common;
using Crayon.Entity.Data;
using Crayon.Entity.Dto;
using Crayon.Entity.Models;
using Crayon.Services.Repository.Interface;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;

namespace Crayon.Services.Repository.Implementation
{
    public class ProjectTaskRepository : IProjectTaskRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectTaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result<ProjectTaskResponse>> AddProjectTask(ProjectTaskRequest request)
        {
            Result<ProjectTaskResponse> result = new();
            try
            {
                var project = await _context.ProjectSet.FirstOrDefaultAsync(x => x.ProjectId == request.ProjectId);
                if (project == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Project Not Found"
                    });
                    return result;
                }
                bool exists = await _context.ProjectTaskSet.AnyAsync(x =>
                x.ProjectId == request.ProjectId && x.TaskName == request.TaskName);
                if (exists)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 400,
                        ErrorMessage = "Task Already Exists"
                    });
                    return result;
                }
                ProjectTask task = new()
                {
                    ProjectId = request.ProjectId,
                    TaskName = request.TaskName
                };
                _context.ProjectTaskSet.Add(task);
                await _context.SaveChangesAsync();
                result.Response = new ProjectTaskResponse
                {
                    ProjectTaskId = task.ProjectTaskId,
                    ProjectId = task.ProjectId,
                    ProjectName = project.ProjectName,
                    TaskName = task.TaskName
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

        public async Task<Result<bool>> DeleteProjectTask(int projectTaskId)
        {
            Result<bool> result = new();
            try
            {
                var task = await _context.ProjectTaskSet.FirstOrDefaultAsync(x => x.ProjectTaskId == projectTaskId);
                if (task == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Project Task Not Found"
                    });

                    return result;
                }
                _context.ProjectTaskSet.Remove(task);
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

        public async Task<Result<List<ProjectTaskResponse>>> GetAllProjectTasks()
        {
            Result<List<ProjectTaskResponse>> result = new();
            try
            {
                var tasks = await (
                    from pt in _context.ProjectTaskSet
                    join p in _context.ProjectSet
                        on pt.ProjectId equals p.ProjectId

                    select new ProjectTaskResponse
                    {
                        ProjectTaskId = pt.ProjectTaskId,
                        ProjectId = pt.ProjectId,
                        ProjectName = p.ProjectName,
                        TaskName = pt.TaskName
                    }
                ).ToListAsync();
                result.Response = tasks;
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

        public async Task<Result<ProjectTaskResponse>> GetProjectTaskById(int projectTaskId)
        {
            Result<ProjectTaskResponse> result = new();
            try
            {
                var task = await (
                    from pt in _context.ProjectTaskSet
                    join p in _context.ProjectSet
                        on pt.ProjectId equals p.ProjectId

                    where pt.ProjectTaskId == projectTaskId

                    select new ProjectTaskResponse
                    {
                        ProjectTaskId = pt.ProjectTaskId,
                        ProjectId = p.ProjectId,
                        ProjectName = p.ProjectName,
                        TaskName = pt.TaskName
                    }).FirstOrDefaultAsync();
                if (task == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Project Task Not Found"
                    });
                    return result;
                }
                result.Response = task;
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

        public async Task<Result<List<ProjectTaskResponse>>>GetTasksByProject(int projectId)
        {
            Result<List<ProjectTaskResponse>> result = new();

            try
            {
                var tasks = await _context.ProjectTaskSet
                    .Where(x => x.ProjectId == projectId)
                    .Select(x => new ProjectTaskResponse
                    {
                        ProjectTaskId = x.ProjectTaskId,
                        ProjectId = x.ProjectId,
                        TaskName = x.TaskName
                    })
                    .ToListAsync();

                result.Response = tasks;

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
        public async Task<Result<bool>> UpdateProjectTask(int projectTaskId,ProjectTaskRequest request)
        {
            Result<bool> result = new();
            try
            {
                var task = await _context.ProjectTaskSet.FirstOrDefaultAsync(x => x.ProjectTaskId == projectTaskId);
                if (task == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Project Task Not Found"
                    });
                    return result;
                }

                task.ProjectId = request.ProjectId;
                task.TaskName = request.TaskName;
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
