using Crayon.Entity.Common;
using Crayon.Entity.Dto;
using Crayon.Services.GenericHttpClient;
using Crayon.Services.GenericHttpClients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Crayon.Client.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IGenericHttpClient _client;

        public EmployeeController(IGenericHttpClient client)
        {
            _client = client;
        }

        public async Task<IActionResult> Index()
        {
            var result =
                await _client.GetAsync<
                    Result<List<EmployeeResponse>>>
                (
                    ApiConstant.GetVisibleEmployees
                );

            return View(result);
        }

        public async Task<IActionResult> Details(int id)
        {
            var result =
                await _client.GetAsync<
                    Result<EmployeeResponse>>
                (
                    ApiConstant.GetEmployeeById + id
                );

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadDropdowns();
            return View(new EmployeeRequest());
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeRequest model)
        {
            var result =
                await _client.PostAsAsync<
                    Result<UserResponse>>
                (
                    ApiConstant.RegisterEmployee,
                    model
                );

            if (result.Error.Any())
            {
                foreach (var error in result.Error)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.ErrorMessage);
                }

                await LoadDropdowns(model.DepartmentId, model.DesignationId, model.WorkplaceId, 0, model.RoleName);

                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result =
                await _client.GetAsync<
                    Result<EmployeeResponse>>
                (
                    ApiConstant.GetEmployeeById + id
                );

            if (result.Response == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var model = new EmployeeRequest
            {
                FirstName          = result.Response.FirstName,
                LastName           = result.Response.LastName,
                EmployeeCode       = result.Response.EmployeeCode,
                PhoneNumber        = result.Response.PhoneNumber,
                DepartmentId       = result.Response.DepartmentId,
                DesignationId      = result.Response.DesignationId,
                WorkplaceId        = result.Response.WorkplaceId,
                ReportingManagerId = result.Response.ReportingManagerId
            };

            await LoadDropdowns(
                model.DepartmentId,
                model.DesignationId,
                model.WorkplaceId,
                model.ReportingManagerId ?? 0
            );

            ViewBag.Id = id;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EmployeeRequest model)
        {
            var result =
                await _client.PutAsAsync<Result<bool>>
                (
                    ApiConstant.UpdateEmployee + id,
                    model
                );

            if (result.Error.Any())
            {
                foreach (var error in result.Error)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.ErrorMessage);
                }

                await LoadDropdowns(model.DepartmentId, model.DesignationId, model.WorkplaceId);

                ViewBag.Id = id;

                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _client.DeleteAsAsync<Result<bool>>
            (
                ApiConstant.DeleteEmployee + id
            );

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> MyTeam()
        {
            string userId = User.FindFirst("UserId")?.Value;

            var result =
                await _client.GetAsync<
                    Result<List<EmployeeResponse>>>
                (
                    $"{ApiConstant.GetMyTeam}/{userId}"
                );

            return View(result);
        }

        private async Task LoadDropdowns(int selDept = 0,int selDes  = 0, int selWp   = 0, int selMgr  = 0, string selRole = null)
        {
            var departments =await _client.GetAsync<Result<List<DepartmentResponse>>>
                (
                    ApiConstant.GetAllDepartments
                );

            ViewBag.Departments =
                new SelectList(
                    departments.Response ?? new List<DepartmentResponse>(),
                    "DepartmentId",
                    "DepartmentName",
                    selDept
                );

            var designations =
                await _client.GetAsync<
                    Result<List<DesignationResponse>>>
                (
                    ApiConstant.GetAllDesignations
                );

            ViewBag.Designations =
                new SelectList(
                    designations.Response ?? new List<DesignationResponse>(),
                    "DesignationId",
                    "DesignationName",
                    selDes
                );

            var workplaces =
                await _client.GetAsync<
                    Result<List<WorkplaceResponse>>>
                (
                    ApiConstant.GetAllWorkplaces
                );

            ViewBag.Workplaces =
                new SelectList(
                    workplaces.Response ?? new List<WorkplaceResponse>(),
                    "WorkplaceId",
                    "Name",
                    selWp
                );

            var employees =
                await _client.GetAsync<
                    Result<List<EmployeeResponse>>>
                (
                    ApiConstant.GetAllEmployee
                );

            ViewBag.Managers =
                new SelectList(
                    employees.Response ?? new List<EmployeeResponse>(),
                    "EmployeeId",
                    "FullName",
                    selMgr
                );

            var roles = await _client.GetAsync<List<string>>(ApiConstant.GetAllRoles);

            ViewBag.Roles = new SelectList(
                roles ?? new List<string>(),
                selRole
            );
        }
    }
}
