using Crayon.Entity.Common;
using Crayon.Entity.Dto;
using Crayon.Services.GenericHttpClient;
using Crayon.Services.GenericHttpClients;
using Microsoft.AspNetCore.Mvc;


namespace Crayon.Client.Controllers
{
    public class WorkplaceController : Controller
    {
        private readonly IGenericHttpClient _client;

        public WorkplaceController(
            IGenericHttpClient client)
        {
            _client = client;
        }

        public async Task<IActionResult> Index()
        {
            var result =
                await _client.GetAsync<
                    Result<List<WorkplaceResponse>>>
                (
                    ApiConstant.GetAllWorkplaces
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
            WorkplaceRequest request)
        {
            var result =
                await _client.PostAsAsync<
                    Result<WorkplaceResponse>>
                (
                    ApiConstant.AddWorkplace,
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
            var result = await _client.GetAsync<Result<WorkplaceResponse>>
            (
                $"{ApiConstant.GetWorkplaceById}/{id}"
            );

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result =
                await _client.GetAsync<
                    Result<WorkplaceResponse>>
                (
                    $"{ApiConstant.GetWorkplaceById}/{id}"
                );

            if (result.Response == null)
            {
                return RedirectToAction(nameof(Index));
            }

            WorkplaceRequest model = new()
            {
                Name = result.Response.Name,
                Address = result.Response.Address
            };

            ViewBag.Id = id;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(
            int id,
            WorkplaceRequest request)
        {
            var result =
                await _client.PutAsAsync<
                    Result<bool>>
                (
                    $"{ApiConstant.UpdateWorkplace}/{id}",
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
                $"{ApiConstant.DeleteWorkplace}/{id}"
            );

            return RedirectToAction(nameof(Index));
        }
    }
}