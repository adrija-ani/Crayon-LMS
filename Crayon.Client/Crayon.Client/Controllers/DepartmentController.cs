using Crayon.Entity.Common;
using Crayon.Entity.Dto;
using Crayon.Services.GenericHttpClient;
using Crayon.Services.GenericHttpClients;
using Microsoft.AspNetCore.Mvc;

namespace Crayon.Client.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IGenericHttpClient _client;

        public DepartmentController(
            IGenericHttpClient client)
        {
            _client = client;
        }

        public async Task<IActionResult> Index()
        {
            var result =
                await _client.GetAsync<
                    Result<List<DepartmentResponse>>>
                (
                    ApiConstant.GetAllDepartments
                );

            return View(result);
        }

        public async Task<IActionResult> Details(int id)
        {
            var result =
                await _client.GetAsync<
                    Result<DepartmentResponse>>
                (
                    ApiConstant.GetDepartmentById + id
                );

            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            DepartmentRequest model)
        {
            await _client.PostAsAsync<
                Result<DepartmentResponse>>
            (
                ApiConstant.AddDepartment,
                model
            );

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result =
                await _client.GetAsync<
                    Result<DepartmentResponse>>
                (
                    ApiConstant.GetDepartmentById + id
                );

            return View(result.Response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(
            int id,
            DepartmentRequest model)
        {
            await _client.PutAsAsync<Result<bool>>
            (
                ApiConstant.UpdateDepartment + id,
                model
            );

            return RedirectToAction(nameof(Index));
        }
    }
}