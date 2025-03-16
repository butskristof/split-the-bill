namespace SplitTheBill.Api;

internal static class DependencyInjection
{
    internal static IServiceCollection AddApi(this IServiceCollection services)
    {
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
}