using Crayon.Entity.Dto;
using Crayon.Services.Repository.Implementation;
using Microsoft.AspNetCore.Mvc;

namespace Crayon.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkplaceController : ControllerBase
    {
        private readonly IWorkplaceRepository _workplaceRepository;

        public WorkplaceController(IWorkplaceRepository workplaceRepository)
        {
            _workplaceRepository = workplaceRepository;
        }

        [HttpPost("AddWorkplace")]
        public async Task<IActionResult> AddWorkplace(
            [FromBody] WorkplaceRequest request)
        {
            var result = await _workplaceRepository
                .AddWorkplace(request);

            return Ok(result);
        }

        [HttpGet("GetAllWorkplaces")]
        public async Task<IActionResult> GetAllWorkplaces()
        {
            var result = await _workplaceRepository.GetAllWorkplaces();
            return Ok(result);
        }

        [HttpGet("GetWorkplaceById/{id}")]
        public async Task<IActionResult> GetWorkplaceById(int id)
        {
            var result = await _workplaceRepository.GetWorkplaceById(id);
            return Ok(result);
        }

        [HttpPut("UpdateWorkplace/{id}")]
        public async Task<IActionResult> UpdateWorkplace(
            int id,
            [FromBody] WorkplaceRequest request)
        {
            var result = await _workplaceRepository.UpdateWorkplace(id, request);
            return Ok(result);
        }

        [HttpDelete("DeleteWorkplace/{id}")]
        public async Task<IActionResult> DeleteWorkplace(int id)
        {
            var result = await _workplaceRepository.DeleteWorkplace(id);
            return Ok(result);
        }
    }
}
