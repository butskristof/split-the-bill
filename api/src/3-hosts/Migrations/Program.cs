using Microsoft.EntityFrameworkCore;
using SplitTheBill.Application.Common.Authentication;
using SplitTheBill.Migrations;
using SplitTheBill.Persistence;
using SplitTheBill.ServiceDefaults;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<AppDbContext>("postgres-database");
builder.Services.AddSingleton<IAuthenticationInfo, AuthenticationInfo>();

var host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Starting database migration");
    
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // dbContext.Database.EnsureCreated();
    logger.LogInformation("Ensured database is created");
    dbContext.Database.Migrate();
    logger.LogInformation("Applied migrations");
    
    logger.LogInformation("Database migration completed successfully");
}