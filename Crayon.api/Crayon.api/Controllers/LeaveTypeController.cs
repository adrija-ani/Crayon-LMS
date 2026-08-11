using Microsoft.AspNetCore.Mvc;
using Crayon.Entity.Dto;
using Crayon.Services.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Crayon.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveTypeController : ControllerBase
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;

        public LeaveTypeController(ILeaveTypeRepository leaveTypeRepository)
        {
            _leaveTypeRepository = leaveTypeRepository;
        }

        [HttpPost("AddLeaveType")]
        public async Task<IActionResult> AddLeaveType(
            [FromBody] LeaveTypeRequest request)
        {
            var result = await _leaveTypeRepository.AddLeaveType(request);
            return Ok(result);
        }

        [HttpGet("GetAllLeaveTypes")]
        public async Task<IActionResult> GetAllLeaveTypes()
        {
            var result = await _leaveTypeRepository.GetAllLeaveTypes();
            return Ok(result);
        }

        [HttpGet("GetLeaveTypeById/{id}")]
        public async Task<IActionResult> GetLeaveTypeById(int id)
        {
            var result = await _leaveTypeRepository.GetLeaveTypeById(id);
            return Ok(result);
        }

        [HttpPut("UpdateLeaveType/{id}")]
        public async Task<IActionResult> UpdateLeaveType(int id,[FromBody] LeaveTypeRequest request)
        {
            var result = await _leaveTypeRepository.UpdateLeaveType(id, request);
            return Ok(result);
        }

        [HttpDelete("DeleteLeaveType/{id}")]
        public async Task<IActionResult> DeleteLeaveType(int id)
        {
            var result =
                await _leaveTypeRepository.DeleteLeaveType(id);

            return Ok(result);
        }
    }
}
