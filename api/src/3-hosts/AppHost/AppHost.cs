using SplitTheBill.AppHost.Constants;

var builder = DistributedApplication.CreateBuilder(args);

#region Database

var databaseMigrations = builder.AddProject<Projects.DatabaseMigrations>(
    Resources.DatabaseMigrations
);

#endregion

#region API

var api = builder
    .AddProject<Projects.Api>(Resources.Api)
    .WithHttpHealthCheck("/health")
    .WaitForCompletion(databaseMigrations);

#endregion

#region Frontend


#endregion

builder.Build().Run();
