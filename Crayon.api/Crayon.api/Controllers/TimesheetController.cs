using Crayon.Entity.Dto;
using Crayon.Services.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Crayon.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimesheetController : ControllerBase
    {
        private readonly ITimesheetRepository _timesheetRepository;

        public TimesheetController(ITimesheetRepository timesheetRepository)
        {
            _timesheetRepository = timesheetRepository;
        }

        [HttpPost("AddTimesheet")]
        public async Task<IActionResult> AddTimesheet([FromBody] TimesheetRequest request)
        {
            var result = await _timesheetRepository.AddTimesheet(request);
            return Ok(result);
        }

        [HttpGet("GetAllTimesheets")]
        public async Task<IActionResult> GetAllTimesheets()
        {
            var result = await _timesheetRepository.GetAllTimesheets();
            return Ok(result);
        }

        [HttpGet("GetTimesheetById/{id}")]
        public async Task<IActionResult> GetTimesheetById(int id)
        {
            var result = await _timesheetRepository.GetTimesheetById(id);
            return Ok(result);
        }

        [HttpGet("GetEmployeeTimesheets/{userId}")]
        public async Task<IActionResult> GetEmployeeTimesheets(string userId)
        {
            var result = await _timesheetRepository.GetEmployeeTimesheets(userId);
            return Ok(result);
        }

        [HttpPut("UpdateTimesheet/{id}")]
        public async Task<IActionResult> UpdateTimesheet(
            int id,
            [FromBody] TimesheetRequest request)
        {
            var result = await _timesheetRepository.UpdateTimesheet(id, request);
            return Ok(result);
        }

        [HttpDelete("DeleteTimesheet/{id}")]
        public async Task<IActionResult> DeleteTimesheet(int id)
        {
            var result = await _timesheetRepository.DeleteTimesheet(id);
            return Ok(result);
        }
    }
}