using Crayon.Entity.Common;
using Crayon.Entity.Dto;
using Crayon.Services.GenericHttpClient;
using Crayon.Services.GenericHttpClients;
using Microsoft.AspNetCore.Mvc;

namespace Crayon.Client.Controllers
{
    public class LeaveTypeController : Controller
    {
        private readonly IGenericHttpClient _client;

        public LeaveTypeController(
            IGenericHttpClient client)
        {
            _client = client;
        }

        public async Task<IActionResult> Index()
        {
            var result =
                await _client.GetAsync<
                    Result<List<LeaveTypeResponse>>>
                (
                    ApiConstant.GetAllLeaveTypes
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
            LeaveTypeRequest request)
        {
            var result =
                await _client.PostAsAsync<
                    Result<LeaveTypeResponse>>
                (
                    ApiConstant.AddLeaveType,
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

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var result =
                await _client.GetAsync<
                    Result<LeaveTypeResponse>>
                (
                    $"{ApiConstant.GetLeaveTypeById}/{id}"
                );

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result =
                await _client.GetAsync<
                    Result<LeaveTypeResponse>>
                (
                    $"{ApiConstant.GetLeaveTypeById}/{id}"
                );

            if (result.Response == null)
            {
                return RedirectToAction(nameof(Index));
            }

            LeaveTypeRequest model = new()
            {
                LeaveName = result.Response.LeaveName,
                DefaultDays = result.Response.DefaultDays
            };

            ViewBag.Id = id;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(
            int id,
            LeaveTypeRequest request)
        {
            var result =
                await _client.PutAsAsync<
                    Result<bool>>
                (
                    $"{ApiConstant.UpdateLeaveType}/{id}",
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
                $"{ApiConstant.DeleteLeaveType}/{id}"
            );

            return RedirectToAction(nameof(Index));
        }
    }
}