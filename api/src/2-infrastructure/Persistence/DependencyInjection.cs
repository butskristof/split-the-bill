using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
        DbConnection? connection = null
    )
    {
        services
            .AddDbContext<AppDbContext>(builder =>
            {
                if (connectionString is not null)
                    builder.UseNpgsql(connectionString, GetDbContextOptionsBuilder());
                else if (connection is not null)
                    builder.UseNpgsql(connection, GetDbContextOptionsBuilder());
                else
                    throw new ArgumentException("Missing connection details to set up persistence");
            })
            .AddScoped<IAppDbContext, AppDbContext>();

        services.AddHealthChecks().AddDbContextCheck<AppDbContext>();

        return services;
    }
}
