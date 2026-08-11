using Crayon.Client.Models;
using Crayon.Entity.Common;
using Crayon.Entity.Dto;
using Crayon.Services.GenericHttpClient;
using Crayon.Services.GenericHttpClients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Crayon.Client.Controllers
{
    public class LeaveApplicationController : Controller
    {
        private readonly IGenericHttpClient _client;

        public LeaveApplicationController(
            IGenericHttpClient client)
        {
            _client = client;
        }

        // Managers/Supervisors see only their direct reports; Admins see all
        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirst("UserId")?.Value;

            bool isAdmin = User.IsInRole("SystemAdministrator") || User.IsInRole("HRAdministrator");

            string endpoint = isAdmin
                ? ApiConstant.GetAllLeaveApplications
                : $"{ApiConstant.GetManagerLeaveApplications}/{userId}";

            var result =
                await _client.GetAsync<
                    Result<List<LeaveApplicationResponse>>>(endpoint);

            return View(result);
        }


        [HttpGet]
        public async Task<IActionResult> Apply()
        {
            string userId = User.FindFirst("UserId")?.Value;

            var balances =
                await _client.GetAsync<
                    Result<List<LeaveBalanceResponse>>>
                (
                    $"{ApiConstant.GetEmployeeLeaveBalances}/{userId}"
                );

            var employeeBalances = balances?.Response ?? new List<LeaveBalanceResponse>();

            await LoadLeaveTypeDropdown(employeeBalances);

            var model = new LeaveApplyViewModel
            {
                LeaveApplication = new LeaveApplicationRequest(),
                LeaveBalances    = employeeBalances
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Apply(
            LeaveApplyViewModel model)
        {
            string userId = User.FindFirst("UserId")?.Value;

            model.LeaveApplication.UserId = userId;

            // UserId is always set server-side, never a form field
            ModelState.Remove("LeaveApplication.UserId");

            var result =
                await _client.PostAsAsync<
                    Result<LeaveApplicationResponse>>
                (
                    ApiConstant.ApplyLeave,
                    model.LeaveApplication
                );

            if (result.Error.Any())
            {
                foreach (var error in result.Error)
                {
                    ModelState.AddModelError("", error.ErrorMessage);
                }

                var balances =
                    await _client.GetAsync<
                        Result<List<LeaveBalanceResponse>>>
                    (
                        $"{ApiConstant.GetEmployeeLeaveBalances}/{userId}"
                    );

                model.LeaveBalances = balances?.Response ?? new List<LeaveBalanceResponse>();

                await LoadLeaveTypeDropdown(model.LeaveBalances);

                return View(model);
            }

            return RedirectToAction(nameof(MyLeaves));
        }


        public async Task<IActionResult> Details(int id)
        {
            var result =
                await _client.GetAsync<
                    Result<LeaveApplicationResponse>>
                (
                    $"{ApiConstant.GetLeaveApplicationById}/{id}"
                );

            return View(result);
        }

        
        public async Task<IActionResult> MyLeaves()
        {
            string userId =
                User.FindFirst("UserId")?.Value;

            var result =
                await _client.GetAsync<
                    Result<List<LeaveApplicationResponse>>>
                (
                    $"{ApiConstant.GetEmployeeLeaveApplications}/{userId}"
                );

            return View(result);
        }


        public async Task<IActionResult> Approve(int id)
        {
            await _client.PutAsAsync<
                Result<bool>>
            (
                $"{ApiConstant.ApproveLeave}/{id}",
                null
            );

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Reject(int id)
        {
            await _client.PutAsAsync<
                Result<bool>>
            (
                $"{ApiConstant.RejectLeave}/{id}",
                null
            );
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Cancel(int id)
        {
            await _client.PutAsAsync<
                Result<bool>>
            (
                $"{ApiConstant.CancelLeave}/{id}",
                null
            );

            return RedirectToAction(nameof(MyLeaves));
        }

        private async Task LoadLeaveTypeDropdown(List<LeaveBalanceResponse> employeeBalances)
        {
            if (employeeBalances.Count > 0)
            {
                ViewBag.LeaveTypes = employeeBalances
                    .Select(b => new SelectListItem
                    {
                        Value = b.LeaveTypeId.ToString(),
                        Text  = b.LeaveName
                    })
                    .ToList();
            }
            else
            {
                var leaveTypes = await _client.GetAsync<Result<List<LeaveTypeResponse>>>(
                    ApiConstant.GetAllLeaveTypes);

                ViewBag.LeaveTypes = (leaveTypes?.Response ?? new List<LeaveTypeResponse>())
                    .Select(lt => new SelectListItem
                    {
                        Value = lt.LeaveTypeId.ToString(),
                        Text  = lt.LeaveName
                    })
                    .ToList();
            }
        }
    }
}