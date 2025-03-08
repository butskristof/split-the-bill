using Microsoft.EntityFrameworkCore;
using SplitTheBillPocV2.Data;
using SplitTheBillPocV2.Modules;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDbContext<AppDbContext>(optionsBuilder => optionsBuilder
        .UseNpgsql(builder.Configuration.GetConnectionString("AppDbContext"))
    );

var app = builder.Build();
app.MapGroupsModule();
app.Run();