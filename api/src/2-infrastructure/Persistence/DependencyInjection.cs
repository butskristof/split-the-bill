using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using SplitTheBill.Application.Common.Persistence;

namespace SplitTheBill.Persistence;

public static class DependencyInjection
{
    internal static Action<NpgsqlDbContextOptionsBuilder> GetDbContextOptionsBuilder() =>
        optionsBuilder =>
        {
            optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
        };

    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        string? connectionString = null,
        DbConnection? connection = null,
        IEnumerable<string>? healthCheckTags = null
    )
    {
        services.AddDbContext<AppDbContext>(builder =>
        {
            if (connectionString is not null)
                builder.UseNpgsql(connectionString, GetDbContextOptionsBuilder());
            else if (connection is not null)
                builder.UseNpgsql(connection, GetDbContextOptionsBuilder());
            else
                throw new ArgumentException("Missing connection details to set up persistence");
        });
        services.AddPersistenceServices(healthCheckTags);

        return services;
    }

    public static IHostApplicationBuilder AddPersistence(
        this IHostApplicationBuilder builder,
        string connectionName,
        IEnumerable<string>? healthCheckTags = null
    )
    {
        builder.AddNpgsqlDbContext<AppDbContext>(
            connectionName,
            settings =>
            {
                // registered manually below to add the ready tag
                settings.DisableHealthChecks = true;
            }
        );
        builder.Services.AddPersistenceServices(healthCheckTags);
        return builder;
    }

    private static IServiceCollection AddPersistenceServices(
        this IServiceCollection services,
        IEnumerable<string>? healthCheckTags = null
    )
    {
        services.AddScoped<IAppDbContextInitializer, AppDbContextInitializer>();
        services.AddScoped<IAppDbContext, AppDbContext>();
        services.AddHealthChecks().AddDbContextCheck<AppDbContext>(tags: healthCheckTags);

        return services;
    }
}
