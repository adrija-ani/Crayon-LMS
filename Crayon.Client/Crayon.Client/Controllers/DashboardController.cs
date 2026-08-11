using Crayon.Entity.Common;
using Crayon.Entity.Dto;
using Crayon.Services.GenericHttpClient;
using Crayon.Services.GenericHttpClients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crayon.Client.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IGenericHttpClient _client;

        public DashboardController(IGenericHttpClient client)
        {
            _client = client;
        }

        public IActionResult Index()
        {
            if (User.IsInRole("SystemAdministrator"))
                return RedirectToAction(nameof(SystemAdmin));

            if (User.IsInRole("HRAdministrator"))
                return RedirectToAction(nameof(HRAdmin));

            if (User.IsInRole("Manager"))
                return RedirectToAction(nameof(Manager));

            if (User.IsInRole("Supervisor"))
                return RedirectToAction(nameof(Supervisor));

            return RedirectToAction(nameof(Employee));
        }

        [Authorize(Roles = "SystemAdministrator")]
        public async Task<IActionResult> SystemAdmin()
        {
            ViewBag.EmployeeCount    = await SafeGet<int>(ApiConstant.GetEmployeeCount);
            ViewBag.DepartmentCount  = await SafeGet<int>(ApiConstant.GetDepartmentCount);
            ViewBag.ProjectCount     = await SafeGet<int>(ApiConstant.GetProjectCount);
            ViewBag.PendingLeaveCount = await SafeGet<int>(ApiConstant.GetPendingLeaveCount);

            return View();
        }

        [Authorize(Roles = "HRAdministrator")]
        public async Task<IActionResult> HRAdmin()
        {
            ViewBag.EmployeeCount    = await SafeGet<int>(ApiConstant.GetEmployeeCount);
            ViewBag.PendingLeaveCount = await SafeGet<int>(ApiConstant.GetPendingLeaveCount);

            return View();
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Manager()
        {
            string userId = User.FindFirst("UserId")?.Value;
            var leaves = await _client.GetAsync<Result<List<LeaveApplicationResponse>>>(
                $"{ApiConstant.GetManagerLeaveApplications}/{userId}");
            ViewBag.PendingTeamLeaves = leaves?.Response?.Count(x => x.Status == "Pending") ?? 0;
            ViewBag.TotalTeamLeaves   = leaves?.Response?.Count ?? 0;
            return View();
        }

        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> Supervisor()
        {
            string userId = User.FindFirst("UserId")?.Value;
            var leaves = await _client.GetAsync<Result<List<LeaveApplicationResponse>>>(
                $"{ApiConstant.GetManagerLeaveApplications}/{userId}");
            ViewBag.PendingTeamLeaves = leaves?.Response?.Count(x => x.Status == "Pending") ?? 0;
            return View();
        }

        public async Task<IActionResult> Employee()
        {
            string userId = User.FindFirst("UserId")?.Value;

            var leaves = await _client.GetAsync<Result<List<LeaveApplicationResponse>>>(
                $"{ApiConstant.GetEmployeeLeaveApplications}/{userId}");

            int pending = leaves?.Response?
                .Count(x => x.Status == "Pending") ?? 0;

            ViewBag.PendingLeaveCount = pending;

            return View();
        }

        private async Task<T> SafeGet<T>(string url)
        {
            try
            {
                return await _client.GetAsync<T>(url);
            }
            catch
            {
                return default;
            }
        }
    }
}
