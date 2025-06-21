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

#region Redis

var redis = builder
    .AddRedis(Resources.Redis)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume();

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
    .WaitFor(api)
    .WithReference(redis)
    .WaitFor(redis);

// secret parameters
var oidcClientId = builder.AddParameter(FrontendConfiguration.OidcClientId.Name, secret: true);
var oidcClientSecret = builder.AddParameter(
    FrontendConfiguration.OidcClientSecret.Name,
    secret: true
);
var oidcTokenKey = builder.AddParameter(FrontendConfiguration.OidcTokenKey.Name, secret: true);
var oidcSessionSecret = builder.AddParameter(
    FrontendConfiguration.OidcSessionSecret.Name,
    secret: true
);
var oidcAuthSessionSecret = builder.AddParameter(
    FrontendConfiguration.OidcAuthSessionSecret.Name,
    secret: true
);

// non-secret parameters
var oidcOpenIdConfig = builder.AddParameter(FrontendConfiguration.OidcOpenIdConfiguration.Name);
var oidcAuthUrl = builder.AddParameter(FrontendConfiguration.OidcAuthorizationUrl.Name);
var oidcTokenUrl = builder.AddParameter(FrontendConfiguration.OidcTokenUrl.Name);
var oidcUserInfoUrl = builder.AddParameter(FrontendConfiguration.OidcUserInfoUrl.Name);
var oidcLogoutUrl = builder.AddParameter(FrontendConfiguration.OidcLogoutUrl.Name);

frontend
    // Dynamic service discovery URLs
    // use http to avoid certificate issues in dev mode
    .WithEnvironment(
        FrontendConfiguration.BackendBaseUrl.EnvironmentVariable,
        api.GetEndpoint("http")
    )
    .WithEnvironment(
        FrontendConfiguration.OidcRedirectUri.EnvironmentVariable,
        () =>
        {
            var endpoint = frontend.GetEndpoint("http");
            return $"{endpoint.Url}/auth/oidc/callback";
        }
    )
    .WithEnvironment(FrontendConfiguration.OidcClientId.EnvironmentVariable, oidcClientId)
    .WithEnvironment(FrontendConfiguration.OidcClientSecret.EnvironmentVariable, oidcClientSecret)
    .WithEnvironment(
        FrontendConfiguration.OidcOpenIdConfiguration.EnvironmentVariable,
        oidcOpenIdConfig
    )
    .WithEnvironment(FrontendConfiguration.OidcAuthorizationUrl.EnvironmentVariable, oidcAuthUrl)
    .WithEnvironment(FrontendConfiguration.OidcTokenUrl.EnvironmentVariable, oidcTokenUrl)
    .WithEnvironment(FrontendConfiguration.OidcUserInfoUrl.EnvironmentVariable, oidcUserInfoUrl)
    .WithEnvironment(FrontendConfiguration.OidcLogoutUrl.EnvironmentVariable, oidcLogoutUrl)
    .WithEnvironment(FrontendConfiguration.OidcTokenKey.EnvironmentVariable, oidcTokenKey)
    .WithEnvironment(FrontendConfiguration.OidcSessionSecret.EnvironmentVariable, oidcSessionSecret)
    .WithEnvironment(
        FrontendConfiguration.OidcAuthSessionSecret.EnvironmentVariable,
        oidcAuthSessionSecret
    )
    .WithEnvironment(
        FrontendConfiguration.RedisHost.EnvironmentVariable,
        () => redis.Resource.PrimaryEndpoint.Host
    )
    .WithEnvironment(
        FrontendConfiguration.RedisPort.EnvironmentVariable,
        () => redis.Resource.PrimaryEndpoint.Port.ToString()
    )
    .WithEnvironment(
        FrontendConfiguration.RedisPassword.EnvironmentVariable,
        () => redis.Resource.PasswordParameter?.Value ?? string.Empty
    );

#endregion

builder.Build().Run();
