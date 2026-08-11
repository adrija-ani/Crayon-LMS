using Crayon.Entity.Dto;
using Crayon.Services.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Crayon.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceRepository _attendanceRepository;

        public AttendanceController(
            IAttendanceRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }

        [HttpPost("CheckIn")]
        public async Task<IActionResult> CheckIn(
            AttendanceRequest request)
        {
            var result = await _attendanceRepository
                .CheckIn(request);

            return Ok(result);
        }

        [HttpPut("CheckOut/{userId}")]
        public async Task<IActionResult> CheckOut(string userId)
        {
            var result = await _attendanceRepository.CheckOut(userId);

            return Ok(result);
        }

        [HttpGet("GetAllAttendance")]
        public async Task<IActionResult> GetAllAttendance()
        {
            var result = await _attendanceRepository
                .GetAllAttendance();

            return Ok(result);
        }

        [HttpGet("GetAttendanceById/{id}")]
        public async Task<IActionResult> GetAttendanceById(
            int id)
        {
            var result = await _attendanceRepository
                .GetAttendanceById(id);

            return Ok(result);
        }

        [HttpGet("GetEmployeeAttendance/{userId}")]
        public async Task<IActionResult> GetEmployeeAttendance(string userId)
        {
            var result =
                await _attendanceRepository
                    .GetEmployeeAttendance(userId);

            return Ok(result);
        }
    }
}