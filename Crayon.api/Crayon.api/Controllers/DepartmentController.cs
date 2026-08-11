using Crayon.Entity.Dto;
using Crayon.Services.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Crayon.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository _departmentRepository;
        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        [HttpPost("AddDepartment")]
        public async Task<IActionResult> AddDepartment([FromBody] DepartmentRequest request)
        {
            var result = await _departmentRepository.AddDepartment(request);
            return Ok(result);
        }


        [HttpGet("GetAllDepartments")]
        public async Task<IActionResult> GetAllDepartments()
        {
            var result = await _departmentRepository.GetAllDepartments();
            return Ok(result);
        }


        [HttpGet("GetDepartmentById/{id}")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            var result = await _departmentRepository.GetDepartmentById(id);
            return Ok(result);
        }


        [HttpPut("UpdateDepartment/{id}")]
        public async Task<IActionResult> UpdateDepartment(int id,[FromBody] DepartmentRequest request)
        {
            var result = await _departmentRepository.UpdateDepartment(id, request);
            return Ok(result);
        }
        
        
    }
}
