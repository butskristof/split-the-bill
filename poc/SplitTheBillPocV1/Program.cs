using Microsoft.EntityFrameworkCore;
using SplitTheBillPocV1.Data;
using SplitTheBillPocV1.Modules.Groups;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDbContext<AppDbContext>(optionsBuilder => optionsBuilder
        .UseNpgsql(builder.Configuration.GetConnectionString("AppDbContext"))
    );

var app = builder.Build();
app.MapGroupsModule();
app.Run();