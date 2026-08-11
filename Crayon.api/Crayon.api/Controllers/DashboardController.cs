using Crayon.Entity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crayon.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("GetEmployeeCount")]
        public async Task<IActionResult> GetEmployeeCount()
        {
            var count = await _context.EmployeeSet.CountAsync();
            return Ok(count);
        }
        [HttpGet("GetDepartmentCount")]
        public async Task<IActionResult> GetDepartmentCount()
        {
            var count = await _context.DepartmentSet.CountAsync();
            return Ok(count);
        }
        [HttpGet("GetProjectCount")]
        public async Task<IActionResult> GetProjectCount()
        {
            var count = await _context.ProjectSet.CountAsync();
            return Ok(count);
        }
        [HttpGet("GetPendingLeaveCount")]
        public async Task<IActionResult> GetPendingLeaveCount()
        {
            var count = await _context.LeaveApplicationSet
                .CountAsync(x => x.Status == "Pending");

            return Ok(count);
        }
    }
}
