using Crayon.Entity.Common;
using Crayon.Entity.Dto;
using Crayon.Services.GenericHttpClient;
using Crayon.Services.GenericHttpClients;
using Microsoft.AspNetCore.Mvc;

namespace Crayon.Client.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IGenericHttpClient _client;

        public ProjectController(IGenericHttpClient client)
        {
            _client = client;
        }

        public async Task<IActionResult> Index()
        {
            var result =
                await _client.GetAsync<
                    Result<List<ProjectResponse>>>
                (
                    ApiConstant.GetAllProjects
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
            ProjectRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result =
                await _client.PostAsAsync<
                    Result<ProjectResponse>>
                (
                    ApiConstant.AddProject,
                    request
                );

            if (result.Error.Any())
            {
                foreach (var error in result.Error)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.ErrorMessage);
                }

                return View(request);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var result =
                await _client.GetAsync<
                    Result<ProjectResponse>>
                (
                    $"{ApiConstant.GetProjectById}/{id}"
                );

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result =
                await _client.GetAsync<
                    Result<ProjectResponse>>
                (
                    $"{ApiConstant.GetProjectById}/{id}"
                );

            if (result.Response == null)
            {
                return RedirectToAction(nameof(Index));
            }

            ProjectRequest model = new()
            {
                ProjectName = result.Response.ProjectName,
                Description = result.Response.Description
            };

            ViewBag.Id = id;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(
            int id,
            ProjectRequest request)
        {
            var result =
                await _client.PutAsAsync<
                    Result<bool>>
                (
                    $"{ApiConstant.UpdateProject}/{id}",
                    request
                );

            if (result.Error.Any())
            {
                foreach (var error in result.Error)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.ErrorMessage);
                }

                ViewBag.Id = id;

                return View(request);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _client.DeleteAsAsync<Result<bool>>
            (
                $"{ApiConstant.DeleteProject}/{id}"
            );

            return RedirectToAction(nameof(Index));
        }
    }
}
