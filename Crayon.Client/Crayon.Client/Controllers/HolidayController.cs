using Crayon.Entity.Common;
using Crayon.Entity.Dto;
using Crayon.Services.GenericHttpClient;
using Crayon.Services.GenericHttpClients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crayon.Client.Controllers
{
    public class HolidayController : Controller
    {
        private readonly IGenericHttpClient _client;

        public HolidayController(
            IGenericHttpClient client)
        {
            _client = client;
        }

        public async Task<IActionResult> Index()
        {
            var result =
                await _client.GetAsync<
                    Result<List<HolidayResponse>>>
                (
                    ApiConstant.GetAllHolidays
                );

            return View(result);
        }

        [HttpGet]
        [Authorize(Roles = "SystemAdministrator,HRAdministrator")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "SystemAdministrator,HRAdministrator")]
        public async Task<IActionResult> Create(
            HolidayRequest request)
        {
            var result =
                await _client.PostAsAsync<
                    Result<HolidayResponse>>
                (
                    ApiConstant.AddHoliday,
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
                    Result<HolidayResponse>>
                (
                    $"{ApiConstant.GetHolidayById}/{id}"
                );

            return View(result);
        }

        [HttpGet]
        [Authorize(Roles = "SystemAdministrator,HRAdministrator")]
        public async Task<IActionResult> Edit(int id)
        {
            var result =
                await _client.GetAsync<
                    Result<HolidayResponse>>
                (
                    $"{ApiConstant.GetHolidayById}/{id}"
                );

            if (result.Response == null)
            {
                return RedirectToAction(nameof(Index));
            }

            HolidayRequest model = new()
            {
                HolidayName = result.Response.HolidayName,
                HolidayDate = result.Response.HolidayDate,
                Country = result.Response.Country
            };

            ViewBag.Id = id;

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "SystemAdministrator,HRAdministrator")]
        public async Task<IActionResult> Edit(
            int id,
            HolidayRequest request)
        {
            var result =
                await _client.PutAsAsync<
                    Result<bool>>
                (
                    $"{ApiConstant.UpdateHoliday}/{id}",
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

        [Authorize(Roles = "SystemAdministrator,HRAdministrator")]
        public async Task<IActionResult> Delete(int id)
        {
            await _client.DeleteAsAsync<
                Result<bool>>
            (
                $"{ApiConstant.DeleteHoliday}/{id}"
            );

            return RedirectToAction(nameof(Index));
        }
    }
}