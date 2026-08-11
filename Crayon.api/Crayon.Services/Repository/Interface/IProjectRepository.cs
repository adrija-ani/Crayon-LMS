using Crayon.Entity.Common;
using Crayon.Entity.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Crayon.Services.Repository.Interface
{
    public interface IProjectRepository
    {
        Task<Result<ProjectResponse>> AddProject(ProjectRequest request);
        Task<Result<List<ProjectResponse>>> GetAllProjects();
        Task<Result<ProjectResponse>> GetProjectById(int projectId);
        Task<Result<bool>> UpdateProject(int projectId,ProjectRequest request);
        Task<Result<bool>> DeleteProject(int projectId);
    }
}
