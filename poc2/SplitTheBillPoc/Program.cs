using Microsoft.EntityFrameworkCore;
using SplitTheBillPoc.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDbContext<AppDbContext>(optionsBuilder => optionsBuilder
        .UseNpgsql(builder.Configuration.GetConnectionString("AppDbContext"))
    );

var app = builder.Build();

app.Run();