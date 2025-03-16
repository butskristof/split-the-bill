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