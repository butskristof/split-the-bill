using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Trace;

namespace SplitTheBill.Persistence;

public interface IAppDbContextInitializer
{
    Task InitializeAsync(CancellationToken cancellationToken = default);
}

internal sealed class AppDbContextInitializer : IAppDbContextInitializer, IDisposable
{
    private readonly ILogger<AppDbContextInitializer> _logger;
    private readonly AppDbContext _dbContext;
    private readonly ActivitySource _activitySource;

    public AppDbContextInitializer(
        ILogger<AppDbContextInitializer> logger,
        AppDbContext dbContext,
        IHostEnvironment hostEnvironment
    )
    {
        _logger = logger;
        _dbContext = dbContext;
        _activitySource = new ActivitySource(hostEnvironment.ApplicationName);
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        // ReSharper disable once ExplicitCallerInfoArgument
        using var activity = _activitySource.StartActivity("Database Initialization");

        try
        {
            _logger.LogInformation("Initializing application database context...");

            await EnsureDatabaseAsync(cancellationToken);
            await RunMigrationsAsync(cancellationToken);

            _logger.LogInformation("Database context initialized successfully");
        }
        catch (Exception ex)
        {
            activity?.RecordException(ex);
            _logger.LogError(ex, "An error occurred during database initialization");
            throw;
        }
    }

    private async Task EnsureDatabaseAsync(CancellationToken cancellationToken)
    {
        var dbCreator = _dbContext.GetService<IRelationalDatabaseCreator>();

        var strategy = _dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Create the database if it does not exist.
            // Do this first so there is then a database to start a transaction against.
            if (!await dbCreator.ExistsAsync(cancellationToken))
            {
                _logger.LogInformation("Database does not exist. Creating database...");
                await dbCreator.CreateAsync(cancellationToken);
                _logger.LogInformation("Database created successfully");
            }
            else
            {
                _logger.LogInformation("Database already exists");
            }
        });
    }

    private async Task RunMigrationsAsync(CancellationToken cancellationToken)
    {
        var strategy = _dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            var migrations = (
                await _dbContext.Database.GetPendingMigrationsAsync(cancellationToken)
            ).ToList();

            if (migrations.Count != 0)
            {
                _logger.LogInformation(
                    "Applying {Count} pending migrations: {Migrations}",
                    migrations.Count,
                    string.Join(", ", migrations)
                );

                await _dbContext.Database.MigrateAsync(cancellationToken);
                _logger.LogInformation("Successfully applied database migrations");
            }
            else
            {
                _logger.LogInformation("No pending migrations found - database is up to date");
            }
        });
    }

    public void Dispose()
    {
        _activitySource.Dispose();
    }
}
