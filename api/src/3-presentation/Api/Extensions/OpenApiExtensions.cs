using Scalar.AspNetCore;

namespace SplitTheBill.Api.Extensions;

internal static class OpenApiExtensions
{
    internal static IEndpointRouteBuilder MapOpenApiEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapOpenApi();
        app.MapScalarApiReference(options => options
            .WithDarkMode(false)
        );

        return app;
    }
}