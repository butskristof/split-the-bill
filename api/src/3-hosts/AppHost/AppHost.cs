using SplitTheBill.AppHost.Constants;
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

// secret parameters
var oidcClientId = builder.AddParameter(FrontendEnvironment.OidcClientId, secret: true);
var oidcClientSecret = builder.AddParameter(FrontendEnvironment.OidcClientSecret, secret: true);
var oidcTokenKey = builder.AddParameter(FrontendEnvironment.OidcTokenKey, secret: true);
var oidcSessionSecret = builder.AddParameter(FrontendEnvironment.OidcSessionSecret, secret: true);
var oidcAuthSessionSecret = builder.AddParameter(
    FrontendEnvironment.OidcAuthSessionSecret,
    secret: true
);

// non-secret parameters
var oidcOpenIdConfig = builder.AddParameter(FrontendEnvironment.OidcOpenIdConfiguration);
var oidcAuthUrl = builder.AddParameter(FrontendEnvironment.OidcAuthorizationUrl);
var oidcTokenUrl = builder.AddParameter(FrontendEnvironment.OidcTokenUrl);
var oidcUserInfoUrl = builder.AddParameter(FrontendEnvironment.OidcUserInfoUrl);
var oidcLogoutUrl = builder.AddParameter(FrontendEnvironment.OidcLogoutUrl);

frontend
    // Dynamic service discovery URLs
    // use http to avoid certificate issues in dev mode
    .WithEnvironment(
        FrontendEnvironment.EnvironmentVariables[FrontendEnvironment.BackendBaseUrl],
        api.GetEndpoint("http")
    )
    .WithEnvironment(
        FrontendEnvironment.EnvironmentVariables[FrontendEnvironment.OidcRedirectUri],
        () =>
        {
            var endpoint = frontend.GetEndpoint("http");
            return $"{endpoint.Url}/auth/oidc/callback";
        }
    )
    .WithEnvironment(
        FrontendEnvironment.EnvironmentVariables[FrontendEnvironment.OidcClientId],
        oidcClientId
    )
    .WithEnvironment(
        FrontendEnvironment.EnvironmentVariables[FrontendEnvironment.OidcClientSecret],
        oidcClientSecret
    )
    .WithEnvironment(
        FrontendEnvironment.EnvironmentVariables[FrontendEnvironment.OidcOpenIdConfiguration],
        oidcOpenIdConfig
    )
    .WithEnvironment(
        FrontendEnvironment.EnvironmentVariables[FrontendEnvironment.OidcAuthorizationUrl],
        oidcAuthUrl
    )
    .WithEnvironment(
        FrontendEnvironment.EnvironmentVariables[FrontendEnvironment.OidcTokenUrl],
        oidcTokenUrl
    )
    .WithEnvironment(
        FrontendEnvironment.EnvironmentVariables[FrontendEnvironment.OidcUserInfoUrl],
        oidcUserInfoUrl
    )
    .WithEnvironment(
        FrontendEnvironment.EnvironmentVariables[FrontendEnvironment.OidcLogoutUrl],
        oidcLogoutUrl
    )
    .WithEnvironment(
        FrontendEnvironment.EnvironmentVariables[FrontendEnvironment.OidcTokenKey],
        oidcTokenKey
    )
    .WithEnvironment(
        FrontendEnvironment.EnvironmentVariables[FrontendEnvironment.OidcSessionSecret],
        oidcSessionSecret
    )
    .WithEnvironment(
        FrontendEnvironment.EnvironmentVariables[FrontendEnvironment.OidcAuthSessionSecret],
        oidcAuthSessionSecret
    );

#endregion

builder.Build().Run();
