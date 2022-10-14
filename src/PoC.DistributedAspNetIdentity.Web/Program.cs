using Microsoft.AspNetCore.Authentication.Cookies;
using PoC.DistributedAspNetIdentity.Web.Exceptions.Filters;
using PoC.DistributedAspNetIdentity.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(o => o.Filters.Add(typeof(HttpExceptionFilter)));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = builder.Configuration.GetValue("Authentication:ExpirationTime", TimeSpan.FromMinutes(20));
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/forbidden/";
        options.LoginPath = "/login/";
        options.LogoutPath = "/logout/";
    });

// In production, the React files will be served from this directory
builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "ClientApp/build";
});

builder.Services.AddHttpClient<IUsersApiClient, UsersApiClient>(o =>
{
    o.BaseAddress = builder.Configuration.GetValue<Uri>("Api:BaseUrl");
    // Demo API key client authentication. Provide your own authentication.
    o.DefaultRequestHeaders.Add("X-APIKEY", builder.Configuration.GetValue<string>("Api:ApiKey", "demo"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app
        .UseHsts()
        .UseHttpsRedirection();
}

app
    .UseStaticFiles()
    .UseSpaStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    })
    .UseSpa(spa =>
    {
        spa.Options.SourcePath = "ClientApp";

        if (app.Environment.IsDevelopment() && !DevSpaProxyDisabled())
        {
            spa.UseProxyToSpaDevelopmentServer("http://localhost:3000");
        }
    });

app.Run();

bool DevSpaProxyDisabled()
{
    return app.Configuration.GetValue("DisableProxyDevServer", false);
}