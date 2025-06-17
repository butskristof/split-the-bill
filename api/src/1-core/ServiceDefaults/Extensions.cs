using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using SplitTheBill.ServiceDefaults.Constants;

namespace SplitTheBill.ServiceDefaults;

// Adds common .NET Aspire services: service discovery, resilience, health checks, and OpenTelemetry.
// This project should be referenced by each service project in your solution.
// To learn more about using this project, see https://aka.ms/dotnet/aspire/service-defaults
public static class Extensions
{
    public static TBuilder AddServiceDefaults<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        builder.ConfigureOpenTelemetry();

        builder.AddDefaultHealthChecks();

        builder.Services.AddServiceDiscovery();

        builder.Services.ConfigureHttpClientDefaults(http =>
        {
            // Turn on resilience by default
            http.AddStandardResilienceHandler();

            // Turn on service discovery by default
            http.AddServiceDiscovery();
        });

        // Uncomment the following to restrict the allowed schemes for service discovery.
        // builder.Services.Configure<ServiceDiscoveryOptions>(options =>
        // {
        //     options.AllowedSchemes = ["https"];
        // });

        return builder;
    }

    #region OpenTelemetry

    private static TBuilder ConfigureOpenTelemetry<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });

        builder
            .Services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation();
            })
            .WithTracing(tracing =>
            {
                tracing
                    .AddSource(builder.Environment.ApplicationName)
                    .AddAspNetCoreInstrumentation(options =>
                        // Exclude health check requests from tracing
                        options.Filter = context =>
                            !context.Request.Path.StartsWithSegments(
                                HealthCheckConstants.Endpoints.BasePath
                            )
                    )
                    // Uncomment the following line to enable gRPC instrumentation (requires the OpenTelemetry.Instrumentation.GrpcNetClient package)
                    //.AddGrpcClientInstrumentation()
                    .AddHttpClientInstrumentation();
            });

        builder.AddOpenTelemetryExporters();

        return builder;
    }

    private static TBuilder AddOpenTelemetryExporters<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        var useOtlpExporter = builder.Configuration.HasValue(
            ConfigurationConstants.OtelExporterEndpoint
        );

        if (useOtlpExporter)
            builder.Services.AddOpenTelemetry().UseOtlpExporter();

        // Uncomment the following lines to enable the Azure Monitor exporter (requires the Azure.Monitor.OpenTelemetry.AspNetCore package)
        // if (
        //     builder.Configuration.HasValue(
        //         ConfigurationConstants.ApplicationInsightsConnectionString
        //     )
        // )
        // {
        //     builder.Services.AddOpenTelemetry().UseAzureMonitor();
        // }

        return builder;
    }

    #endregion

    #region Health checks

    private static TBuilder AddDefaultHealthChecks<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        builder
            .Services.AddHealthChecks()
            // add a minimal health check that always returns healthy to indicate that the service is alive
            .AddCheck(
                HealthCheckConstants.Names.Self,
                () => HealthCheckResult.Healthy(),
                [HealthCheckConstants.Tags.Live]
            );

        return builder;
    }

    #endregion

    public static WebApplication MapDefaultEndpoints(this WebApplication app)
    {
        // Adding health checks endpoints to applications in non-development environments has security implications.
        // See https://aka.ms/dotnet/aspire/healthchecks for details before enabling these endpoints in non-development environments.
        if (app.Environment.IsDevelopment())
        {
            // Map health checks for live and ready endpoints
            // live is a basic check to indicate service is responding
            // ready indicates more complete health, e.g. connectivity to external dependencies such as
            // database, service bus, ...
            app.MapHealthCheckForTag(HealthCheckConstants.Tags.Live)
                .MapHealthCheckForTag(HealthCheckConstants.Tags.Ready);
        }

        return app;
    }

    private static WebApplication MapHealthCheckForTag(this WebApplication app, string tag)
    {
        app.MapHealthChecks(
            $"{HealthCheckConstants.Endpoints.BasePath}/{tag}",
            new HealthCheckOptions { Predicate = healthCheck => healthCheck.Tags.Contains(tag) }
        );

        return app;
    }
}
