using Microsoft.EntityFrameworkCore;
using SplitTheBillPocV4.Data;
using SplitTheBillPocV4.Modules;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDbContext<AppDbContext>(optionsBuilder => optionsBuilder
        .UseNpgsql(builder.Configuration.GetConnectionString("AppDbContext"))
    );

var app = builder.Build();
app.MapGroupsModule();
app.Run();
