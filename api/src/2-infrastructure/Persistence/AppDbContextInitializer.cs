using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace SplitTheBill.Persistence;

public interface IAppDbContextInitializer
{
    Task InitializeAsync(CancellationToken cancellationToken = default);
}

internal sealed class AppDbContextInitializer : IAppDbContextInitializer
{
    private readonly ILogger<AppDbContextInitializer> _logger;
    private readonly AppDbContext _dbContext;

    public AppDbContextInitializer(ILogger<AppDbContextInitializer> logger, AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Initializing application database context...");

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

        _logger.LogInformation("Database context initialized successfully");
    }
}
