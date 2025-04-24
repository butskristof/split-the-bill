using Microsoft.Extensions.DependencyInjection;

namespace SplitTheBill.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // the default implementation of TimeProvider can be registered as a singleton
        services.AddSingleton(TimeProvider.System);

        return services;
    }
}
