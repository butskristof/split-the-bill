using SplitTheBill.Api;
using SplitTheBill.Api.Extensions;
using SplitTheBill.Api.Modules;
using SplitTheBill.Application;
using SplitTheBill.Infrastructure;
using SplitTheBill.Persistence;
using SplitTheBill.ServiceDefaults;
using SplitTheBill.ServiceDefaults.Constants;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddConfiguration().AddApplication().AddInfrastructure();
builder.AddPersistence(Resources.AppDb);
builder.Services.AddApi();

var app = builder.Build();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

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

app.MapDefaultEndpoints();
app.MapOpenApiEndpoints();
app.MapMembersEndpoints().MapGroupsEndpoints();

app.Run();
