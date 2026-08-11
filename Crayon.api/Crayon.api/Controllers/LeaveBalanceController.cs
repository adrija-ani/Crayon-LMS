using Crayon.Entity.Dto;
using Crayon.Services.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Crayon.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveBalanceController : ControllerBase
    {
        private readonly ILeaveBalanceRepository _leaveBalanceRepository;

        public LeaveBalanceController(ILeaveBalanceRepository leaveBalanceRepository)
        {
            _leaveBalanceRepository = leaveBalanceRepository;
        }

        [HttpPost("AddLeaveBalance")]
        public async Task<IActionResult> AddLeaveBalance([FromBody] LeaveBalanceRequest request)
        {
            var result = await _leaveBalanceRepository.AddLeaveBalance(request);
            return Ok(result);
        }

        [HttpGet("GetAllLeaveBalances")]
        public async Task<IActionResult> GetAllLeaveBalances()
        {
            var result = await _leaveBalanceRepository.GetAllLeaveBalances();
            return Ok(result);
        }

        [HttpGet("GetLeaveBalanceById/{id}")]
        public async Task<IActionResult> GetLeaveBalanceById(int id)
        {
            var result = await _leaveBalanceRepository.GetLeaveBalanceById(id);
            return Ok(result);
        }

        [HttpPut("UpdateLeaveBalance/{id}")]
        public async Task<IActionResult> UpdateLeaveBalance(
            int id,
            [FromBody] LeaveBalanceRequest request)
        {
            var result = await _leaveBalanceRepository.UpdateLeaveBalance(id, request);
            return Ok(result);
        }

        [HttpDelete("DeleteLeaveBalance/{id}")]
        public async Task<IActionResult> DeleteLeaveBalance(int id)
        {
            var result = await _leaveBalanceRepository.DeleteLeaveBalance(id);
            return Ok(result);
        }

        [HttpGet("GetEmployeeLeaveBalances/{userId}")]
        public async Task<IActionResult>GetEmployeeLeaveBalances(string userId)
        {
            var result = await _leaveBalanceRepository.GetEmployeeLeaveBalances(userId);
            return Ok(result);
        }
    }
}