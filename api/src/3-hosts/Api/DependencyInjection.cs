using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SplitTheBill.Api.Common.Authentication;
using SplitTheBill.Api.Constants;
using SplitTheBill.Api.Extensions;
using SplitTheBill.Application.Common.Authentication;
using SplitTheBill.Application.Common.Configuration;
using SplitTheBill.Application.Common.Validation;

namespace SplitTheBill.Api;

internal static class DependencyInjection
{
    #region Configuration

    internal static IServiceCollection AddConfiguration(this IServiceCollection services)
    {
        services
            .AddValidatedSettings<CorsSettings>()
            .AddValidatedSettings<AuthenticationSettings>();

        return services;
    }

    private static IServiceCollection AddValidatedSettings<TOptions>(
        this IServiceCollection services
    )
        where TOptions : class, ISettings =>
        services.AddValidatedSettings<TOptions>(TOptions.SectionName);

    private static IServiceCollection AddValidatedSettings<TOptions>(
        this IServiceCollection services,
        string sectionName
    )
        where TOptions : class
    {
        services
            .AddOptions<TOptions>()
            .BindConfiguration(sectionName)
            .FluentValidateOptions()
            .ValidateOnStart();

        return services;
    }

    #endregion

    #region API

    internal static IServiceCollection AddApi(this IServiceCollection services)
    {
        // add runtime-generated OpenAPI documentation
        services.AddOpenApi(
            ApiConstants.OpenApiDocumentName,
            options =>
            {
                options.CustomSchemaIds(type =>
                {
                    // build the full type name by traversing up its declaring types, this will result in e.g.
                    // "CreateExpense.Request.Participant"
                    var schemaId = type.Name;
                    for (
                        Type current = type;
                        current.DeclaringType is not null;
                        current = current.DeclaringType
                    )
                        schemaId = $"{current.DeclaringType.Name}.{schemaId}";

                    return schemaId;
                });

                options.AddDocumentTransformer(
                    (document, _, _) =>
                    {
                        var scheme = new OpenApiSecurityScheme
                        {
                            BearerFormat = "JSON Web Token",
                            Description = "Bearer authentication using a JWT",
                            Scheme = "bearer",
                            Type = SecuritySchemeType.Http,
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme,
                            },
                        };

                        document.Components ??= new OpenApiComponents();
                        document.Components.SecuritySchemes ??=
                            new Dictionary<string, OpenApiSecurityScheme>();
                        document.Components.SecuritySchemes[scheme.Reference.Id] = scheme;

                        // Also register the scheme with the security requirements
                        document.SecurityRequirements ??= [];
                        document.SecurityRequirements.Add(
                            new OpenApiSecurityRequirement { [scheme] = [] }
                        );

                        return Task.FromResult(document);
                    }
                );
            }
        );

        // add support for ProblemDetails to handle failed requests
        // it's effectively a default implementation of the IProblemDetailsService and can be
        // overridden if desired
        // it adds a default problem details response for all client and server error responses (400-599)
        // that don't have body content yet
        services.AddProblemDetails();

        services.AddHealthChecks();

        services
            .AddAuthentication()
            .AddJwtBearer(options =>
            {
                using var serviceProvider = services.BuildServiceProvider();
                var configuration = serviceProvider
                    .GetRequiredService<IOptions<AuthenticationSettings>>()
                    .Value;

                options.Authority = configuration.Authority;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidAudiences = configuration.Audiences,
                    ValidateIssuer = true,
                    ValidIssuer = configuration.Authority,
                    ValidateIssuerSigningKey = true,
                    RequireSignedTokens = true,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                };
            });
        services.AddAuthorization();
        services
            .AddHttpContextAccessor()
            .AddSingleton<IAuthenticationInfo, ApiAuthenticationInfo>();

        services.AddCors(options =>
        {
            using var serviceProvider = services.BuildServiceProvider();
            var settings = serviceProvider.GetRequiredService<IOptions<CorsSettings>>().Value;
            if (!settings.AllowCors)
                return;

            options.AddDefaultPolicy(policy =>
                policy.WithOrigins(settings.AllowedOrigins).AllowAnyHeader().AllowAnyMethod()
            );
        });

        return services;
    }

    #endregion
}
