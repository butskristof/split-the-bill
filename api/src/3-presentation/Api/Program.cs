using SplitTheBill.Api;
using SplitTheBill.Api.Modules;
using SplitTheBill.Application;
using SplitTheBill.Application.Common.Constants;
using SplitTheBill.Infrastructure;
using SplitTheBill.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddInfrastructure()
    .AddPersistence(
        builder.Configuration.GetConnectionString(ConfigurationConstants.AppDbContextConnectionStringKey)
    )
    .AddApi();

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

app
    .MapMembersEndpoints();

app.Run();