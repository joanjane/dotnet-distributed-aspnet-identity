var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// In production, the React files will be served from this directory
builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "ClientApp/build";
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

app
    .UseRouting()
    .UseEndpoints(endpoints =>
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