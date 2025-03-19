using Serilog;
using SplitTheBill.Api;
using SplitTheBill.Api.Modules;
using SplitTheBill.Application;
using SplitTheBill.Application.Common.Constants;
using SplitTheBill.Infrastructure;
using SplitTheBill.Persistence;

Log.Logger = new LoggerConfiguration()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services
        .AddConfiguration()
        .AddApplication()
        .AddInfrastructure()
        .AddPersistence(
            builder.Configuration.GetConnectionString(ConfigurationConstants.AppDbContextConnectionStringKey)
        )
        .AddApi();

    builder.Host
        .UseSerilog((context, configuration) => configuration
            .Enrich.FromLogContext()
            .ReadFrom.Configuration(context.Configuration)
        );

    var app = builder.Build();

    app
        // the default exception handler will catch unhandled exceptions and return 
        // them as ProblemDetails with status code 500 Internal Server Error
        .UseExceptionHandler()
        // the status code pages will map additional failed requests (outside of
        // those throwing exceptions) to responses with ProblemDetails body content
        // this includes 404, method not allowed, ... (all status codes between 400 and 599)
        // keep in mind that this middleware will only activate if the body is empty when
        // it reaches it
        .UseStatusCodePages();

    app.MapHealthChecks("/health");
    app.MapOpenApi();
    app
        .MapMembersEndpoints()
        .MapGroupsEndpoints();

    app.Run();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
    throw;
}
finally
{
    Log.CloseAndFlush();
}