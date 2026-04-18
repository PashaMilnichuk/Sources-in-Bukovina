using Microsoft.AspNetCore.Mvc;

namespace CarpathianCrown.Web.Models;

public static class HttpContextAuthExtensions
{
    public static WebAuth GetAuth(this HttpContext ctx)
    {
        var token = ctx.Session.GetString(SessionKeys.JwtToken);
        var role = ctx.Session.GetString(SessionKeys.Role);
        var email = ctx.Session.GetString(SessionKeys.Email);
        var userIdStr = ctx.Session.GetString(SessionKeys.UserId);
        int? userId = int.TryParse(userIdStr, out var v) ? v : null;

        return new WebAuth(!string.IsNullOrWhiteSpace(token), token, role, email, userId);
    }

    public static string GetLang(this HttpContext ctx)
        => ctx.Session.GetString(SessionKeys.Lang) ?? "ua";

    public static void SetLang(this HttpContext ctx, string lang)
        => ctx.Session.SetString(SessionKeys.Lang, (lang is "en" or "ua") ? lang : "ua");

    public static IActionResult RequireLogin(this Controller c)
        => c.RedirectToAction("Login", "Auth");

    public static bool IsAdmin(this HttpContext ctx)
        => string.Equals(ctx.Session.GetString(SessionKeys.Role), "Admin", StringComparison.OrdinalIgnoreCase);
}