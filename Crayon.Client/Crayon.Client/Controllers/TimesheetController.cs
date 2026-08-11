using Crayon.Entity.Common;
using Crayon.Entity.Dto;
using Crayon.Services.GenericHttpClient;
using Crayon.Services.GenericHttpClients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;


namespace Crayon.Client.Controllers
{
    public class TimesheetController : Controller
    {
        private readonly IGenericHttpClient _client;

        public TimesheetController(
            IGenericHttpClient client)
        {
            _client = client;
        }

        public async Task<IActionResult> Index()
        {
            var result =
                await _client.GetAsync<
                    Result<List<TimesheetResponse>>>
                (
                    ApiConstant.GetAllTimesheets
                );
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
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
                    "ProjectName"
                );

            var tasks =
                await _client.GetAsync<
                    Result<List<ProjectTaskResponse>>>
                (
                    ApiConstant.GetAllProjectTasks
                );

            ViewBag.Tasks =
                new SelectList(
                    tasks.Response,
                    "ProjectTaskId",
                    "TaskName"
                );

            var model = new TimesheetRequest
            {
                WorkDate = DateTime.Today
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TimesheetRequest request)
        {
            request.UserId =
                User.FindFirstValue("UserId");

            var result =
                await _client.PostAsAsync<
                    Result<TimesheetResponse>>
                (
                    ApiConstant.AddTimesheet,
                    request
                );

            if (result.Error.Any())
            {
                foreach (var error in result.Error)
                {
                    ModelState.AddModelError(
                        "",
                        error.ErrorMessage);
                }

                return View(request);
            }

            return RedirectToAction(nameof(MyTimesheets));
        }
        public async Task<IActionResult> Details(int id)
        {
            var result =
                await _client.GetAsync<
                    Result<TimesheetResponse>>
                (
                    $"{ApiConstant.GetTimesheetById}/{id}"
                );

            return View(result);
        }

        public async Task<IActionResult> MyTimesheets()
        {
            string userId = User.FindFirstValue("UserId");

            var result =
                await _client.GetAsync<
                    Result<List<TimesheetResponse>>>
                (
                    $"{ApiConstant.GetEmployeeTimesheets}/{userId}"
                );

            return View(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetTasksByProject(int projectId)
        {
            var result =
                await _client.GetAsync<
                    Result<List<ProjectTaskResponse>>>
                (
                    $"{ApiConstant.GetTasksByProject}/{projectId}"
                );

            return Json(result.Response);
        }
    }
}