using Crayon.Entity.Common;
using Crayon.Entity.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Crayon.Services.Repository.Interface
{
    public interface IProjectTaskRepository
    {
        Task<Result<ProjectTaskResponse>> AddProjectTask(ProjectTaskRequest request);
        Task<Result<List<ProjectTaskResponse>>> GetAllProjectTasks();
        Task<Result<ProjectTaskResponse>> GetProjectTaskById(int projectTaskId);
        Task<Result<List<ProjectTaskResponse>>> GetTasksByProject(int projectId);
        Task<Result<bool>> UpdateProjectTask(int projectTaskId,ProjectTaskRequest request);
        Task<Result<bool>> DeleteProjectTask(int projectTaskId);

    }
}
