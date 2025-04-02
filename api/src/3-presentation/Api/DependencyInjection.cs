using Microsoft.Extensions.Options;
using SplitTheBill.Api.Common;
using SplitTheBill.Api.Constants;
using SplitTheBill.Api.Extensions;
using SplitTheBill.Application.Common.Authentication;
using SplitTheBill.Application.Common.Configuration;

namespace SplitTheBill.Api;

internal static class DependencyInjection
{
    #region Configuration

    internal static IServiceCollection AddConfiguration(this IServiceCollection services)
    {
        services.AddValidatedSettings<CorsSettings>();
        return services;
    }

    private static IServiceCollection AddValidatedSettings<TOptions>(this IServiceCollection services)
        where TOptions : class, ISettings
        => services.AddValidatedSettings<TOptions>(TOptions.SectionName);

    private static IServiceCollection AddValidatedSettings<TOptions>(this IServiceCollection services,
        string sectionName)
        where TOptions : class
    {
        services
            .AddOptions<TOptions>()
            .BindConfiguration(sectionName);
        // .FluentValidateOptions()
        // .ValidateOnStart();

        return services;
    }

    #endregion

    #region API

    internal static IServiceCollection AddApi(this IServiceCollection services)
    {
        // add runtime-generated OpenAPI documentation
        services.AddOpenApi(ApiConstants.OpenApiDocumentName, options =>
        {
            options.CustomSchemaIds(type =>
            {
                // build the full type name by traversing up its declaring types, this will result in e.g.
                // "CreateExpense.Request.Participant"
                var schemaId = type.Name;
                for (Type current = type; current.DeclaringType is not null; current = current.DeclaringType)
                    schemaId = $"{current.DeclaringType.Name}.{schemaId}";

                return schemaId;
            });
        });

        // add support for ProblemDetails to handle failed requests
        // it's effectively a default implementation of the IProblemDetailsService and can be
        // overridden if desired
        // it adds a default problem details response for all client and server error responses (400-599)
        // that don't have body content yet
        services.AddProblemDetails();

        services
            .AddHealthChecks();

        services
            .AddAuthentication()
            .AddJwtBearer();
        services.AddAuthorization();
        services
            .AddHttpContextAccessor()
            .AddScoped<IAuthenticationInfo, ApiAuthenticationInfo>();

        // TODO remove after BFF/Proxy setup
        services.AddCors(options =>
        {
            using var serviceProvider = services.BuildServiceProvider();
            var settings = serviceProvider
                .GetRequiredService<IOptions<CorsSettings>>()
                .Value;
            if (!settings.AllowCors) return;

            options.AddDefaultPolicy(policy => policy
                .WithOrigins(settings.AllowedOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod()
            );
        });

        return services;
    }

    #endregion
}