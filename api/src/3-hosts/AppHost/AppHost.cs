using SplitTheBill.ServiceDefaults.Constants;

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
    .WithHttpHealthCheck("/health/live")
    .WithHttpHealthCheck("/health/ready");

#endregion

#region Frontend

var frontend = builder
    .AddNpmApp(Resources.Frontend, "../../../../frontend", "dev")
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .WithReference(api)
    .WaitFor(api);

frontend
    // use http to avoid certificate issues in dev mode
    .WithEnvironment("NUXT_BACKEND_BASE_URL", api.GetEndpoint("http"))
    .WithEnvironment(
        "NUXT_OIDC_PROVIDERS_OIDC_REDIRECT_URI",
        () => $"{frontend.GetEndpoint("http")}/auth/oidc/callback"
    );

#endregion

builder.Build().Run();
