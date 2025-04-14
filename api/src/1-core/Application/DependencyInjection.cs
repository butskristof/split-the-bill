using System.Reflection;
using FluentValidation;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using SplitTheBill.Application.Common.Pipeline;

namespace SplitTheBill.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // FluentValidation
        // scan the current assembly for validators and register them with the DI container 
        // make sure to include internal types, since almost all of them should be defined as internal sealed class
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), includeInternalTypes: true);

        services
            .AddMediator(options => { options.ServiceLifetime = ServiceLifetime.Scoped; })
            // add cross-cutting concerns (supporting services) in the pipeline 
            // keep in mind that order of registration matters here
            .AddSingleton(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
            .AddSingleton(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}