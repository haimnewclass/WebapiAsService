using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.ComponentModel;

// Create Host for Worker Service
var host = Host.CreateDefaultBuilder(args).UseWindowsService()    
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<BackgroundWorkerService>();
    })
    .Build();













            //Create WebHost for ASP.NET Core Application
            var webHost = new WebHostBuilder()
                .UseKestrel()
                .ConfigureServices(services =>
                {
                    services.AddControllers();  // Add MVC services
                }).UseUrls("http://localhost:5009", "https://localhost:5001")   
                .Configure(app =>
                {
                    app.UseCors();
                    app.UseRouting();

                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();  // Map controller routes
                    });
                    
                    app.Run(async context =>
                    {
                        await context.Response.WriteAsync("Fallback handler");
                    });
                })
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                })
                .Build();

// Run both concurrently
await Task.WhenAll(host.RunAsync(), webHost.RunAsync());

/*
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationInsightsTelemetry();
//builder.Host.UseWindowsService();

// Configure the HTTP request pipeline.

if (WindowsServiceHelpers.IsWindowsService())
{
    //  app.UseWindowsService();
   // builder.Host.UseWindowsService();

}


builder.WebHost.UseUrls("http://localhost:5000", "https://localhost:5001");
var app = builder.Build();
app.MapControllers();


// Task.Run(() => { app.Run(); });

await Task.WhenAll(host.RunAsync(), app.RunAsync());
*/



var webHost = new WebHostBuilder()
    .UseKestrel()
    .ConfigureServices(services =>
    {
        services.AddControllers();  // Add MVC services
        services.AddEndpointsApiExplorer();

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });
        });

    }).UseUrls(hostUrl)
    .Configure(app =>
    {
        app.UseCors(); // This should be before UseRouting and UseEndpoints
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();  // Map controller routes
        });

        app.Run(async context =>
        {
            await context.Response.WriteAsync("Fallback handler");
        });

    })
    .ConfigureLogging(logging =>
    {
        logging.AddConsole();
    })
    .Build();

await Task.WhenAll(host.RunAsync(), webHost.RunAsync());