using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SplitTheBill.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("AppDbContext");

        services
            .AddDbContext<AppDbContext>(builder => builder
                .UseNpgsql(connectionString, options =>
                {
                    options.MigrationsAssembly(typeof(AppDbContext).Assembly);
                    options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                })
            );

        return services;
    }
}