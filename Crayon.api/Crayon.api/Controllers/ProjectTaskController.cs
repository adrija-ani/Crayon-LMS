using Crayon.Entity.Dto;
using Crayon.Services.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Crayon.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectTaskController : ControllerBase
    {
        private readonly IProjectTaskRepository _projectTaskRepository;

        public ProjectTaskController(
            IProjectTaskRepository projectTaskRepository)
        {
            _projectTaskRepository = projectTaskRepository;
        }

        [HttpPost("AddProjectTask")]
        public async Task<IActionResult> AddProjectTask(
            [FromBody] ProjectTaskRequest request)
        {
            var result = await _projectTaskRepository
                .AddProjectTask(request);

            return Ok(result);
        }

        [HttpGet("GetAllProjectTasks")]
        public async Task<IActionResult> GetAllProjectTasks()
        {
            var result = await _projectTaskRepository
                .GetAllProjectTasks();

            return Ok(result);
        }

        [HttpGet("GetProjectTaskById/{id}")]
        public async Task<IActionResult> GetProjectTaskById(int id)
        {
            var result = await _projectTaskRepository
                .GetProjectTaskById(id);

            return Ok(result);
        }

        [HttpGet("GetTasksByProject/{projectId}")]
        public async Task<IActionResult> GetTasksByProject(int projectId)
        {
            var result =
                await _projectTaskRepository
                    .GetTasksByProject(projectId);

            return Ok(result);
        }

        [HttpPut("UpdateProjectTask/{id}")]
        public async Task<IActionResult> UpdateProjectTask(
            int id,
            [FromBody] ProjectTaskRequest request)
        {
            var result = await _projectTaskRepository
                .UpdateProjectTask(id, request);

            return Ok(result);
        }

        [HttpDelete("DeleteProjectTask/{id}")]
        public async Task<IActionResult> DeleteProjectTask(int id)
        {
            var result = await _projectTaskRepository
                .DeleteProjectTask(id);

            return Ok(result);
        }

        
     
    }
}