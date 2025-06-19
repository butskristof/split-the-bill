using SplitTheBill.Application.Common.Authentication;
using SplitTheBill.DatabaseMigrations;
using SplitTheBill.Persistence;
using SplitTheBill.ServiceDefaults;

var builder = Host.CreateApplicationBuilder(args);
builder.AddServiceDefaults();
builder.AddPersistence();
builder.Services.AddSingleton<IAuthenticationInfo, DummyAuthenticationInfo>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();