using Crayon.Entity.Dto;
using Crayon.Services.Repository.Interface;
using Microsoft.AspNetCore.Mvc;


namespace Crayon.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveApplicationController : ControllerBase
    {
        private readonly ILeaveApplicationRepository _leaveApplicationRepository;

        public LeaveApplicationController(ILeaveApplicationRepository leaveApplicationRepository)
        {
            _leaveApplicationRepository = leaveApplicationRepository;
        }
        [HttpPost("ApplyLeave")]
        public async Task<IActionResult> ApplyLeave([FromBody] LeaveApplicationRequest request)
        {
            var result = await _leaveApplicationRepository
                .ApplyLeave(request);

            return Ok(result);
        }

        [HttpGet("GetAllLeaveApplications")]
        public async Task<IActionResult> GetAllLeaveApplications()
        {
            var result = await _leaveApplicationRepository
                .GetAllLeaveApplications();

            return Ok(result);
        }

        [HttpGet("GetLeaveApplicationById/{id}")]
        public async Task<IActionResult> GetLeaveApplicationById(int id)
        {
            var result = await _leaveApplicationRepository
                .GetLeaveApplicationById(id);

            return Ok(result);
        }

        [HttpPut("ApproveLeave/{id}")]
        public async Task<IActionResult> ApproveLeave(int id)
        {
            var result =
                await _leaveApplicationRepository.ApproveLeave(id);

            return Ok(result);
        }

        [HttpPut("RejectLeave/{id}")]
        public async Task<IActionResult> RejectLeave(int id)
        {
            var result = await _leaveApplicationRepository.RejectLeave(id);
            return Ok(result);
        }

        [HttpGet("GetEmployeeLeaveApplications/{userId}")]
        public async Task<IActionResult> GetEmployeeLeaveApplications(string userId)
        {
            var result =
                await _leaveApplicationRepository.GetEmployeeLeaveApplications(userId);

            return Ok(result);
        }

        [HttpPut("CancelLeave/{id}")]
        public async Task<IActionResult> CancelLeave(int id)
        {
            var result = await _leaveApplicationRepository.CancelLeave(id);
            return Ok(result);
        }

        [HttpGet("GetManagerLeaveApplications/{userId}")]
        public async Task<IActionResult> GetManagerLeaveApplications(string userId)
        {
            var result = await _leaveApplicationRepository.GetManagerLeaveApplications(userId);
            return Ok(result);
        }
    }
}
