namespace SplitTheBill.Migrations;

internal sealed class AppDbContextMigrationService : BackgroundService
{
    private readonly ILogger<AppDbContextMigrationService> _logger;

    public AppDbContextMigrationService(ILogger<AppDbContextMigrationService> logger)
    {
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("HI!");

        _logger.LogInformation("Done");
        return Task.CompletedTask;
    }
}