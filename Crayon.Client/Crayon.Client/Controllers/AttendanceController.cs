using Crayon.Entity.Common;
using Crayon.Entity.Dto;
using Crayon.Services.GenericHttpClient;
using Crayon.Services.GenericHttpClients;
using Microsoft.AspNetCore.Mvc;

namespace Crayon.Client.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly IGenericHttpClient _client;

        public AttendanceController(IGenericHttpClient client)
        {
            _client = client;
        }

        public async Task<IActionResult> MyAttendance()
        {
            string userId = User.FindFirst("UserId")?.Value;

            var result =
                await _client.GetAsync<
                    Result<List<AttendanceResponse>>>
                (
                    $"{ApiConstant.GetEmployeeAttendance}/{userId}"
                );

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> TodayStatus()
        {
            string userId = User.FindFirst("UserId")?.Value;

            var result = await _client.GetAsync<Result<List<AttendanceResponse>>>(
                $"{ApiConstant.GetEmployeeAttendance}/{userId}"
            );

            if (result?.Response == null)
                return Json(new { status = "none" });

            var today = result.Response
                .FirstOrDefault(x => x.AttendanceDate.Date == DateTime.Today);

            if (today == null)
                return Json(new { status = "none" });

            if (today.CheckOutTime.HasValue)
            {
                int totalWorked = (int)(today.CheckOutTime.Value - today.CheckInTime).TotalSeconds;
                return Json(new
                {
                    status        = "completed",
                    checkInTime   = today.CheckInTime.ToString(@"hh\:mm\:ss"),
                    checkOutTime  = today.CheckOutTime.Value.ToString(@"hh\:mm\:ss"),
                    secondsWorked = totalWorked < 0 ? 0 : totalWorked
                });
            }

            int secondsWorked = (int)(DateTime.Now.TimeOfDay - today.CheckInTime).TotalSeconds;

            return Json(new
            {
                status        = "checkedIn",
                checkInTime   = today.CheckInTime.ToString(@"hh\:mm\:ss"),
                secondsWorked = secondsWorked < 0 ? 0 : secondsWorked
            });
        }

        [HttpPost]
        public async Task<IActionResult> CheckIn()
        {
            AttendanceRequest request = new()
            {
                UserId = User.FindFirst("UserId")?.Value
            };

            var result =
                await _client.PostAsAsync<
                    Result<AttendanceResponse>>
                (
                    ApiConstant.CheckIn,
                    request
                );

            if (result.Error.Any())
            {
                TempData["Error"] =
                    result.Error.First().ErrorMessage;
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut()
        {
            string userId = User.FindFirst("UserId")?.Value;

            var result =
                await _client.PutAsAsync<Result<bool>>
                (
                    $"{ApiConstant.CheckOut}/{userId}",
                    null
                );

            return Ok(result);
        }

        public async Task<IActionResult> Index()
        {
            var result =
                await _client.GetAsync<
                    Result<List<AttendanceResponse>>>
                (
                    ApiConstant.GetAllAttendance
                );

            return View(result);
        }

        public async Task<IActionResult> Details(int id)
        {
            var result =
                await _client.GetAsync<
                    Result<AttendanceResponse>>
                (
                    $"{ApiConstant.GetAttendanceById}/{id}"
                );

            return View(result);
        }
    }
}
