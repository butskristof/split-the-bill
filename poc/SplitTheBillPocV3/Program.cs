using Microsoft.EntityFrameworkCore;
using SplitTheBillPocV3.Data;
using SplitTheBillPocV3.Modules;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDbContext<AppDbContext>(optionsBuilder => optionsBuilder
        .UseNpgsql(builder.Configuration.GetConnectionString("AppDbContext"))
    );

var app = builder.Build();
app.MapGroupsModule();
app.Run();
