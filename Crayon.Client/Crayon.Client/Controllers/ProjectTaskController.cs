using Crayon.Entity.Common;
using Crayon.Entity.Dto;
using Crayon.Services.GenericHttpClient;
using Crayon.Services.GenericHttpClients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Crayon.Client.Controllers
{
    public class ProjectTaskController : Controller
    {
        private readonly IGenericHttpClient _client;

        public ProjectTaskController(IGenericHttpClient client)
        {
            _client = client;
        }

        public async Task<IActionResult> Index()
        {
            var result =
                await _client.GetAsync<
                    Result<List<ProjectTaskResponse>>>
                (
                    ApiConstant.GetAllProjectTasks
                );

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadProjects();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            ProjectTaskRequest request)
        {
            if (!ModelState.IsValid)
            {
                await LoadProjects();
                return View(request);
            }

            var result =
                await _client.PostAsAsync<
                    Result<ProjectTaskResponse>>
                (
                    ApiConstant.AddProjectTask,
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

                await LoadProjects();
                return View(request);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var result =
                await _client.GetAsync<
                    Result<ProjectTaskResponse>>
                (
                    $"{ApiConstant.GetProjectTaskById}/{id}"
                );

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result =
                await _client.GetAsync<
                    Result<ProjectTaskResponse>>
                (
                    $"{ApiConstant.GetProjectTaskById}/{id}"
                );

            if (result.Response == null)
            {
                return RedirectToAction(nameof(Index));
            }

            ProjectTaskRequest model = new()
            {
                ProjectId = result.Response.ProjectId,
                TaskName  = result.Response.TaskName
            };

            await LoadProjects(model.ProjectId);

            ViewBag.Id = id;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(
            int id,
            ProjectTaskRequest request)
        {
            var result =
                await _client.PutAsAsync<
                    Result<bool>>
                (
                    $"{ApiConstant.UpdateProjectTask}/{id}",
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

                await LoadProjects(request.ProjectId);

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
                $"{ApiConstant.DeleteProjectTask}/{id}"
            );

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadProjects(int selected = 0)
        {
            var projects =
                await _client.GetAsync<
                    Result<List<ProjectResponse>>>
                (
                    ApiConstant.GetAllProjects
                );

            ViewBag.Projects =
                new SelectList(
                    projects.Response,
                    "ProjectId",
                    "ProjectName",
                    selected
                );
        }
    }
}
