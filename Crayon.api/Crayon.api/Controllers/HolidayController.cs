using Crayon.Entity.Dto;
using Crayon.Services.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Crayon.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HolidayController : ControllerBase
    {
        private readonly IHolidayRepository _holidayRepository;

        public HolidayController(IHolidayRepository holidayRepository)
        {
            _holidayRepository = holidayRepository;
        }

        [HttpPost("AddHoliday")]
        public async Task<IActionResult> AddHoliday(
            [FromBody] HolidayRequest request)
        {
            var result = await _holidayRepository.AddHoliday(request);

            return Ok(result);
        }

        [HttpGet("GetAllHolidays")]
        public async Task<IActionResult> GetAllHolidays()
        {
            var result = await _holidayRepository.GetAllHolidays();

            return Ok(result);
        }

        [HttpGet("GetHolidayById/{id}")]
        public async Task<IActionResult> GetHolidayById(int id)
        {
            var result = await _holidayRepository.GetHolidayById(id);

            return Ok(result);
        }

        [HttpPut("UpdateHoliday/{id}")]
        public async Task<IActionResult> UpdateHoliday(
            int id,
            [FromBody] HolidayRequest request)
        {
            var result = await _holidayRepository.UpdateHoliday(id, request);

            return Ok(result);
        }

        [HttpDelete("DeleteHoliday/{id}")]
        public async Task<IActionResult> DeleteHoliday(int id)
        {
            var result = await _holidayRepository.DeleteHoliday(id);

            return Ok(result);
        }
    }
}