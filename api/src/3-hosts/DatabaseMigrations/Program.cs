using SplitTheBill.Application.Common.Authentication;
using SplitTheBill.DatabaseMigrations;
using SplitTheBill.Persistence;
using SplitTheBill.ServiceDefaults;
using SplitTheBill.ServiceDefaults.Constants;

var builder = Host.CreateApplicationBuilder(args);
builder.AddServiceDefaults();
builder.AddPersistence(Resources.AppDb);
builder.Services.AddSingleton<IAuthenticationInfo, DummyAuthenticationInfo>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();