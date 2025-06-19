using SplitTheBill.AppHost.Constants;

var builder = DistributedApplication.CreateBuilder(args);

#region Database

var postgres = builder
    .AddPostgres(Resources.Postgres)
    .WithPgAdmin()
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume();
var appDb = postgres.AddDatabase(Resources.AppDb);

var databaseMigrations = builder
    .AddProject<Projects.DatabaseMigrations>(Resources.DatabaseMigrations)
    .WithReference(appDb)
    .WaitFor(appDb);

#endregion

#region API

var api = builder
    .AddProject<Projects.Api>(Resources.Api)
    .WithReference(appDb)
    .WaitForCompletion(databaseMigrations)
    .WithHttpHealthCheck("/health");

#endregion

#region Frontend


#endregion

builder.Build().Run();
