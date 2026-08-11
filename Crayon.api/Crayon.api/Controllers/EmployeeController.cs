using Crayon.Entity.Common;
using Crayon.Entity.Dto;
using Crayon.Entity.Models;
using Crayon.Services.Repository.Implementation;
using Crayon.Services.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crayon.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(
            IUserRepository userRepository,
            IEmployeeRepository employeeRepository,
            RoleManager<IdentityRole> roleManager,
            ILogger<EmployeeController> logger)
        {
            _userRepository      = userRepository;
            _employeeRepository  = employeeRepository;
            _roleManager         = roleManager;
            _logger = logger;
        }

        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleManager.Roles
                .Select(r => r.Name)
                .ToListAsync();

            return Ok(roles);
        }

        [HttpPost("RegisterEmployee")]
        public async Task<IActionResult> Register([FromBody] EmployeeRequest request)
        {
            var role = string.IsNullOrWhiteSpace(request.RoleName) ? "Employee" : request.RoleName;
            var result = await _userRepository.Register(request, role);
            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserRequest request)
        {

            var result = await _userRepository.Authorize(request);
            return Ok(result);
        }

        [HttpGet("GetAllEmployees")]
        public async Task<IActionResult> GetAllEmployees()
        {
            _logger.LogInformation("Fetching All Employees");
            var result = await _employeeRepository.GetAllEmployees();
            return Ok(result);
        }

        [HttpDelete("DeleteEmployee/{id}")]

        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var result = await _employeeRepository.DeleteEmployee(id);
            return Ok(result);
        }

        //[Authorize]
        [HttpGet("GetEmployeeById/{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var result = await _employeeRepository.GetEmployeeById(id);
            return Ok(result);
        }

        //[Authorize]
        [HttpPut("UpdateEmployee/{id}")]
        public async Task<IActionResult> UpdateEmployee(int id,[FromBody] EmployeeRequest request)
        {
            var result = await _employeeRepository.UpdateEmployee(id, request);
            return Ok(result);
        }

        [HttpGet("GetEmployeesByDepartment/{departmentId}")]
        public async Task<IActionResult> GetEmployeesByDepartment(int departmentId)
        {
            var result = await _employeeRepository
                .GetEmployeesByDepartment(departmentId);

            return Ok(result);
        }

        [HttpGet("GetMyTeam/{userId}")]
        public async Task<IActionResult> GetMyTeam(string userId)
        {
            var result =
                await _employeeRepository.GetMyTeam(userId);

            return Ok(result);
        }
        [HttpGet]
        [Route("GetVisibleEmployees")]
        public async Task<IActionResult> GetVisibleEmployees()
        {
            try
            {
                var result = await _userRepository.GetVisibleEmployees();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
