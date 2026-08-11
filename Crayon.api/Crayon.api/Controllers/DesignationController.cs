using Crayon.Entity.Dto;
using Crayon.Services.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crayon.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class DesignationController : ControllerBase
    {
        private readonly IDesignationRepository _designationRepository;

        public DesignationController(IDesignationRepository designationRepository)
        {
            _designationRepository = designationRepository;
        }

        [HttpPost("AddDesignation")]
        public async Task<IActionResult> AddDesignation([FromBody] DesignationRequest request)
        {
            var result = await _designationRepository.AddDesignation(request);
            return Ok(result);
        }

        [HttpGet("GetAllDesignations")]
        public async Task<IActionResult> GetAllDesignations()
        {
            var result = await _designationRepository.GetAllDesignations();
            return Ok(result);
        }

        [HttpGet("GetDesignationById/{id}")]
        public async Task<IActionResult> GetDesignationById(int id)
        {
            var result = await _designationRepository.GetDesignationById(id);
            return Ok(result);
        }

        [HttpPut("UpdateDesignation/{id}")]
        public async Task<IActionResult> UpdateDesignation(int id,[FromBody] DesignationRequest request)
        {
            var result = await _designationRepository.UpdateDesignation(id, request);
            return Ok(result);
        }

        [HttpDelete("DeleteDesignation/{id}")]
        public async Task<IActionResult> DeleteDesignation(int id)
        {
            var result = await _designationRepository.DeleteDesignation(id);
            return Ok(result);
        }
    }
}