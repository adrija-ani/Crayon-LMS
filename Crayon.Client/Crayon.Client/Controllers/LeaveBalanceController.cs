using Crayon.Entity.Common;
using Crayon.Entity.Dto;
using Crayon.Services.GenericHttpClient;
using Crayon.Services.GenericHttpClients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Crayon.Client.Controllers
{
    public class LeaveBalanceController : Controller
    {
        private readonly IGenericHttpClient _client;

        public LeaveBalanceController(
            IGenericHttpClient client)
        {
            _client = client;
        }

        private async Task LoadDropdowns(int selEmployee = 0, int selLeaveType = 0)
        {
            var employees = await _client.GetAsync<Result<List<EmployeeResponse>>>(
                ApiConstant.GetAllEmployee);

            ViewBag.Employees = new SelectList(
                employees?.Response ?? new List<EmployeeResponse>(),
                "EmployeeId",
                "FullName",
                selEmployee == 0 ? (object)null : selEmployee);

            var leaveTypes = await _client.GetAsync<Result<List<LeaveTypeResponse>>>(
                ApiConstant.GetAllLeaveTypes);

            ViewBag.LeaveTypes = new SelectList(
                leaveTypes?.Response ?? new List<LeaveTypeResponse>(),
                "LeaveTypeId",
                "LeaveName",
                selLeaveType == 0 ? (object)null : selLeaveType);
        }

        public async Task<IActionResult> Index()
        {
            var result =
                await _client.GetAsync<
                    Result<List<LeaveBalanceResponse>>>
                (
                    ApiConstant.GetAllLeaveBalances
                );

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadDropdowns();
            return View(new LeaveBalanceRequest());
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            LeaveBalanceRequest request)
        {
            var result =
                await _client.PostAsAsync<
                    Result<LeaveBalanceResponse>>
                (
                    ApiConstant.AddLeaveBalance,
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

                await LoadDropdowns(request.EmployeeId, request.LeaveTypeId);
                return View(request);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var result =
                await _client.GetAsync<
                    Result<LeaveBalanceResponse>>
                (
                    $"{ApiConstant.GetLeaveBalanceById}/{id}"
                );

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result =
                await _client.GetAsync<
                    Result<LeaveBalanceResponse>>
                (
                    $"{ApiConstant.GetLeaveBalanceById}/{id}"
                );

            if (result.Response == null)
            {
                return RedirectToAction(nameof(Index));
            }

            LeaveBalanceRequest model = new()
            {
                EmployeeId    = result.Response.EmployeeId,
                LeaveTypeId   = result.Response.LeaveTypeId,
                AvailableDays = result.Response.AvailableDays
            };

            await LoadDropdowns(model.EmployeeId, model.LeaveTypeId);
            ViewBag.Id = id;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(
            int id,
            LeaveBalanceRequest request)
        {
            var result =
                await _client.PutAsAsync<
                    Result<bool>>
                (
                    $"{ApiConstant.UpdateLeaveBalance}/{id}",
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

                await LoadDropdowns(request.EmployeeId, request.LeaveTypeId);
                ViewBag.Id = id;

                return View(request);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _client.DeleteAsAsync<
                Result<bool>>
            (
                $"{ApiConstant.DeleteLeaveBalance}/{id}"
            );

            return RedirectToAction(nameof(Index));
        }
    }
}