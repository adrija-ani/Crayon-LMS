using Crayon.Entity.Common;
using Crayon.Entity.Dto;
using Crayon.Services.GenericHttpClient;
using Crayon.Services.GenericHttpClients;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Crayon.Client.Controllers
{
    public class AccountController : Controller
    {
        private readonly IGenericHttpClient _client;

        public AccountController(IGenericHttpClient client)
        {
            _client = client;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("SystemAdministrator"))
                    return RedirectToAction("SystemAdmin", "Dashboard");

                if (User.IsInRole("HRAdministrator"))
                    return RedirectToAction("HRAdmin", "Dashboard");

                if (User.IsInRole("Manager"))
                    return RedirectToAction("Manager", "Dashboard");

                if (User.IsInRole("Supervisor"))
                    return RedirectToAction("Supervisor", "Dashboard");

                if (User.IsInRole("Employee"))
                    return RedirectToAction("Employee", "Dashboard");

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(
            UserRequest model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _client.PostAsAsync<Result<UserResponse>>(ApiConstant.Login, model);
            if (result.Response == null)
            {
                if (result.Error != null)
                {
                    foreach (var error in result.Error)
                    {
                        ModelState.AddModelError(string.Empty, error.ErrorMessage);
                    }
                }
                return View(model);
            }

            string token = result.Response.Token;
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            List<Claim> claims = new()
            {
                new Claim(
                    ClaimTypes.Name,
                    result.Response.UserName),

                new Claim(
                    ClaimTypes.Email,
                    result.Response.Email),

                new Claim(
                    "UserId",
                    result.Response.UserId),

                new Claim(
                    "token",
                    token)
            };

            var roleClaims =
                jwt.Claims.Where(
                    x => x.Type == ClaimTypes.Role || x.Type == "role");

            foreach (var role in roleClaims)
            {
                claims.Add(
                    new Claim(
                        ClaimTypes.Role,
                        role.Value));
            }

            var identity =
                new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults
                        .AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults
                    .AuthenticationScheme,
                new ClaimsPrincipal(identity),
                new AuthenticationProperties
                {
                    IsPersistent = true
                });
            var roles = roleClaims.FirstOrDefault()?.Value;

            switch (roles)
            {
                case "SystemAdministrator":
                    return RedirectToAction("SystemAdmin", "Dashboard");

                case "HRAdministrator":
                    return RedirectToAction("HRAdmin", "Dashboard");

                case "Manager":
                    return RedirectToAction("Manager", "Dashboard");

                case "Supervisor":
                    return RedirectToAction("Supervisor", "Dashboard");

                case "Employee":
                    return RedirectToAction("Employee", "Dashboard");

                default:
                    return RedirectToAction("Index", "Home");
            }
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults
                    .AuthenticationScheme);

            return RedirectToAction(nameof(Login));
        }

        //    
    }
}