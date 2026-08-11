using Crayon.Entity.Dto;
using Crayon.Services.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Crayon.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectController(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        [HttpPost("AddProject")]
        public async Task<IActionResult> AddProject([FromBody] ProjectRequest request)
        {
            var result = await _projectRepository.AddProject(request);
            return Ok(result);
        }

        [HttpGet("GetAllProjects")]
        public async Task<IActionResult> GetAllProjects()
        {
            var result = await _projectRepository.GetAllProjects();

            return Ok(result);
        }

        [HttpGet("GetProjectById/{id}")]
        public async Task<IActionResult> GetProjectById(int id)
        {
            var result = await _projectRepository.GetProjectById(id);
            return Ok(result);
        }

        [HttpPut("UpdateProject/{id}")]
        public async Task<IActionResult> UpdateProject(int id,[FromBody] ProjectRequest request)
        {
            var result = await _projectRepository.UpdateProject(id, request);
            return Ok(result);
        }

        [HttpDelete("DeleteProject/{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var result = await _projectRepository.DeleteProject(id);
            return Ok(result);
        }
    }
}