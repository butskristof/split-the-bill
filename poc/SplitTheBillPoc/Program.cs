using Microsoft.EntityFrameworkCore;
using SplitTheBillPoc.Data;
using SplitTheBillPoc.Modules.Groups;
using SplitTheBillPoc.Modules.Members;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDbContext<AppDbContext>(optionsBuilder => optionsBuilder
        .UseNpgsql(builder.Configuration.GetConnectionString("AppDbContext"))
    );

var app = builder.Build();

app
    .MapMembersModule()
    .MapGroupsModule();

app.Run();