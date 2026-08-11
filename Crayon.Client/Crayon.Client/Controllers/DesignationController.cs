using Crayon.Entity.Common;
using Crayon.Entity.Dto;
using Crayon.Services.GenericHttpClient;
using Crayon.Services.GenericHttpClients;
using Microsoft.AspNetCore.Mvc;

namespace Crayon.Client.Controllers
{
    public class DesignationController : Controller
    {
        private readonly IGenericHttpClient _client;

        public DesignationController(IGenericHttpClient client)
        {
            _client = client;
        }

        public async Task<IActionResult> Index()
        {
            var result =
                await _client.GetAsync<
                    Result<List<DesignationResponse>>>
                (
                    ApiConstant.GetAllDesignations
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
            DesignationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result =
                await _client.PostAsAsync<
                    Result<DesignationResponse>>
                (
                    ApiConstant.AddDesignation,
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
                    Result<DesignationResponse>>
                (
                    $"{ApiConstant.GetDesignationById}/{id}"
                );

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result =
                await _client.GetAsync<
                    Result<DesignationResponse>>
                (
                    $"{ApiConstant.GetDesignationById}/{id}"
                );

            if (result.Response == null)
            {
                return RedirectToAction(nameof(Index));
            }

            DesignationRequest model = new()
            {
                DesignationName =
                    result.Response.DesignationName
            };

            ViewBag.Id = id;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(
            int id,
            DesignationRequest request)
        {
            var result =
                await _client.PutAsAsync<
                    Result<bool>>
                (
                    $"{ApiConstant.UpdateDesignation}/{id}",
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
        public async Task<IActionResult> Delete(int id)
        {
            await _client.DeleteAsAsync<
                Result<bool>>
            (
                $"{ApiConstant.DeleteDesignation}/{id}"
            );

            return RedirectToAction(nameof(Index));
        }
    }
}