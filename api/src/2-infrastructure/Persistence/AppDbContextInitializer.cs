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

        await _dbContext.Database.MigrateAsync(cancellationToken);
        _logger.LogInformation("Applied database migrations from DbContext");

        _logger.LogInformation("Database context initialized successfully");
    }
}
