using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Portfolio.Components;
using Portfolio.Services;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLocalization();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("lv"),
        new CultureInfo("en")
    };

    options.DefaultRequestCulture = new RequestCulture("lv");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<AdminState>();

var app = builder.Build();

var locOptions = app.Services
    .GetRequiredService<Microsoft.Extensions.Options.IOptions<RequestLocalizationOptions>>()
    .Value;

app.UseRequestLocalization(locOptions);

app.UseStaticFiles();
app.UseAntiforgery();

app.MapGet("/set-culture/{culture}", (string culture, HttpContext httpContext, string? returnUrl) =>
{
    if (culture != "lv" && culture != "en")
    {
        culture = "lv";
    }

    httpContext.Response.Cookies.Append(
        CookieRequestCultureProvider.DefaultCookieName,
        CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
        new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddYears(1)
        });

    return Results.Redirect(string.IsNullOrWhiteSpace(returnUrl) ? "/" : returnUrl);
});

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();