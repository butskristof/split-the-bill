using SplitTheBill.AppHost.Constants;

var builder = DistributedApplication.CreateBuilder(args);

#region Database

var postgres = builder
    .AddPostgres(Resources.Postgres)
    .WithPgAdmin()
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);
var db = postgres.AddDatabase(Resources.Database);
var migrations = builder
    .AddProject<Projects.Migrations>("migrations")
    .WithReference(db)
    .WaitFor(db);

#endregion

#region API

// var api = builder
//     .AddProject<Projects.Api>(Resources.Api)
//     .WithReference(db)
//     .WaitForCompletion(migrations);

#endregion

#region Frontend


#endregion

builder.Build().Run();
