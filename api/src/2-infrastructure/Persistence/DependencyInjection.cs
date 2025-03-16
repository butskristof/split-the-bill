using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using SplitTheBill.Application.Common.Persistence;

namespace SplitTheBill.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, string? connectionString = null,
        DbConnection? connection = null)
    {
        services
            .AddDbContext<AppDbContext>(builder =>
            {
                Action<NpgsqlDbContextOptionsBuilder> sqlServerOptionsAction = optionsBuilder =>
                {
                    optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                };

                if (connectionString is not null)
                    builder.UseNpgsql(connectionString, sqlServerOptionsAction);
                else if (connection is not null)
                    builder.UseNpgsql(connection, sqlServerOptionsAction);
                else
                    throw new ArgumentException("Missing connection details to set up persistence");
            })
            .AddScoped<IAppDbContext, AppDbContext>();

        services
            .AddHealthChecks()
            .AddDbContextCheck<AppDbContext>();

        return services;
    }
}