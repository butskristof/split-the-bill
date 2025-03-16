using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace SplitTheBill.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddMediatR(configuration =>
            {
                // scan the current assembly for requests and handlers and register them with the DI container
                // internals should be included by default, no additional configuration is necessary
                configuration
                    .RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                
                // TODO add behaviours (logging, validation, ...)
            });
        
        return services;
    }
}