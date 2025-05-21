using Microsoft.AspNetCore.Diagnostics;
using MyWebNovel.Api;
using MyWebNovel.Api.Extensions;
using MyWebNovel.Api.Middleware;
using MyWebNovel.Application;
using MyWebNovel.Application.Secutity;
using MyWebNovel.Infrastructure;
using MyWebNovel.Infrastructure.Persistence;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .WriteTo.Console()
            .CreateBootstrapLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);
    Log.Logger.Information("Starting server.");
    builder.Host.UseSerilog((context, services, configuration) =>
    {
        configuration.ReadFrom.Configuration(context.Configuration);
        configuration.ReadFrom.Services(services);
        configuration.Enrich.FromLogContext();
    });

    // Add services to the container.
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi("v1", options =>
    {
        options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
    });

    // Add services to the container.
    builder.AddInfrastructureServices();
    builder.AddApplicationServices();
    builder.AddApiServices();

    var app = builder.Build();

    // Modify log message of serilog 
    app.UseSerilogRequestLogging(configure =>
    {
        configure.MessageTemplate = "HTTP {RequestMethod} {RequestPath} ({UserId}) responded {StatusCode} in {Elapsed:0.0000}ms";
    });

    // Configure the HTTP request pipeline
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();

        // Map OpenAPI and Scalar API References
        app.MapOpenApi();
        app.MapScalarApiReference();

        // Ensure the database is created in a scoped service context
        using var serviceScope = app.Services.CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
    }
    else
    {
        // Production Exception Handler
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature?.Error;

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var errorResponse = new
                {
                    error = "An error occurred while processing your request.",
                    details = exception?.Message
                };

                await context.Response.WriteAsJsonAsync(errorResponse);
            });
        });
    }

    app.UseHttpsRedirection(); // Redirect HTTP to HTTPS

    // Register ExceptionMiddleware here
    app.UseMiddleware<ExceptionMiddleware>();

    app.UseAuthentication();   // Handle user authentication
    app.UseAuthorization();    // Handle user authorization policies

    app.MapAllEndpoints();     // Map all endpoints (controllers or minimal API endpoints)

    app.Run();                 // Start the app
}
catch (Exception ex)
{
    Log.Logger.Fatal(ex, "Host Terminated unexpectedly.");
}
finally
{
    Log.CloseAndFlush();
}
