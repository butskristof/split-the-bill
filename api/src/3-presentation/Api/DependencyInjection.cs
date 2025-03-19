using Microsoft.AspNetCore.OpenApi;
using SplitTheBill.Api.Constants;

namespace SplitTheBill.Api;

internal static class DependencyInjection
{
    #region Configuration

    internal static IServiceCollection AddConfiguration(this IServiceCollection services)
    {
        return services;
    }

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
            // since almost all the in- and outgoing types are named Request and Response, we'll use 
            // the full name of the type as schema ID 
            // start by using the default implementation and only replace the value if it is not null 
            // (this will exclude primitive types which shouldn't explicitly be listed in the specification)
            options.CreateSchemaReferenceId = type =>
            {
                var defaultSchemaReferenceId = OpenApiOptions.CreateDefaultSchemaReferenceId(type);
                if (string.IsNullOrWhiteSpace(defaultSchemaReferenceId)) return null;
                var schemaReferenceId = type.Type.FullName;
                if (string.IsNullOrWhiteSpace(schemaReferenceId)) return null;

                // inner classes such as request are joined with + instead of .
                schemaReferenceId = schemaReferenceId.Replace('+', '.');
                // remove common prefixes (SplitTheBill.Application.Modules e.g.)
                // by splitting them into parts, we can progressively shorten the result and given we 
                // hold to a similar structure (e.g. Application/Modules and Api/Modules), this can
                // effectively result in a uniform collection of schema IDs
                string[] prefixesToDelete =
                [
                    nameof(SplitTheBill),
                    nameof(Application),
                    nameof(Api),
                    nameof(Modules)
                ];
                foreach (var prefix in prefixesToDelete)
                {
                    if (schemaReferenceId.StartsWith(prefix + "."))
                        schemaReferenceId = schemaReferenceId.Remove(0, prefix.Length + 1);
                }

                return schemaReferenceId;
            };
        });

        // add support for ProblemDetails to handle failed requests
        // it's effectively a default implementation of the IProblemDetailsService and can be
        // overridden if desired
        // it adds a default problem details response for all client and server error responses (400-599)
        // that don't have body content yet
        services.AddProblemDetails();

        services
            .AddHealthChecks();

        return services;
    }

    #endregion
}