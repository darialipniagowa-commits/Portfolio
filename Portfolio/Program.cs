using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Portfolio.Components;
using Portfolio.Components.Account;
using Portfolio.Data;
using Portfolio.Services;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContext")
    ?? throw new InvalidOperationException("Connection string 'ApplicationDbContext' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

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

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddScoped<PurchaseRequestService>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
.AddIdentityCookies();

builder.Services.AddIdentityCore<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddSignInManager()
.AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<IdentityUser>, IdentityNoOpEmailSender>();
builder.Services.AddScoped<ArtworkService>();
builder.Services.AddScoped<AdminState>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("Seed");

    try
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

        const string adminRole = "Admin";
        const string adminEmail = "";

        if (!await roleManager.RoleExistsAsync(adminRole))
        {
            await roleManager.CreateAsync(new IdentityRole(adminRole));
        }

        var user = await userManager.FindByEmailAsync(adminEmail);
        if (user is not null && !await userManager.IsInRoleAsync(user, adminRole))
        {
            await userManager.AddToRoleAsync(user, adminRole);
        }
        if (user is not null)
        {
            var roles = await userManager.GetRolesAsync(user);
            Console.WriteLine($"User roles: {string.Join(", ", roles)}");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error while seeding admin role.");
    }
}

var locOptions = app.Services
    .GetRequiredService<Microsoft.Extensions.Options.IOptions<RequestLocalizationOptions>>()
    .Value;

app.UseRequestLocalization(locOptions);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

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

app.MapAdditionalIdentityEndpoints();

app.Run();