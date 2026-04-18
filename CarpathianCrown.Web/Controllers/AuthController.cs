using CarpathianCrown.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarpathianCrown.Web.Controllers;

public class AuthController : Controller
{
    private readonly ApiClient _api;
    public AuthController(ApiClient api) => _api = api;

    [HttpGet]
    public IActionResult Login()
    {
        ViewBag.Auth = HttpContext.GetAuth();
        return View(new LoginViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        try
        {
            var req = new CarpathianCrown.Web.ApiModels.LoginRequest
            {
                Identifier = model.Identifier,
                Password = model.Password
            };

            var resp = await _api.Post<
                CarpathianCrown.Web.ApiModels.LoginRequest,
                CarpathianCrown.Web.Models.AuthResponse
            >("/api/auth/login", req);

            HttpContext.Session.SetString(SessionKeys.JwtToken, resp.Token);
            HttpContext.Session.SetString(SessionKeys.Role, resp.Role);
            HttpContext.Session.SetString(SessionKeys.UserId, resp.UserId.ToString());
            HttpContext.Session.SetString(SessionKeys.Email, model.Identifier);

            if (string.Equals(resp.Role, "Admin", StringComparison.OrdinalIgnoreCase))
                return RedirectToAction("Dashboard", "Admin");

            return RedirectToAction("Index", "Account");
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            ViewBag.Auth = HttpContext.GetAuth();
            return View(model);
        }
    }

    [HttpGet]
    public IActionResult Register()
    {
        ViewBag.Auth = HttpContext.GetAuth();
        return View(new RegisterViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!model.AcceptPersonalData)
        {
            ViewBag.Error = "Потрібно погодитись з обробкою персональних даних";
            return View(model);
        }
        try
        {
            var req = new CarpathianCrown.Web.ApiModels.RegisterRequest
            {
                Email = model.Email,
                Password = model.Password,
                FirstName = model.FirstName,
                LastName = model.LastName,
                AcceptPersonalData = model.AcceptPersonalData
            };

            await _api.Post("/api/auth/register", req);

            return RedirectToAction("Login");
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            ViewBag.Auth = HttpContext.GetAuth();
            return View(model);
        }
    }

    [HttpPost]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}