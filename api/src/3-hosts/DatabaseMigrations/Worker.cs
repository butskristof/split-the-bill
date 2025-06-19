using SplitTheBill.Persistence;

namespace SplitTheBill.DatabaseMigrations;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;

    public Worker(
        ILogger<Worker> logger,
        IServiceProvider serviceProvider,
        IHostApplicationLifetime hostApplicationLifetime)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _hostApplicationLifetime = hostApplicationLifetime;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // var cts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
        // cts.CancelAfter(TimeSpan.FromSeconds(5));
        //
        // try
        // {
        //     while (!cts.Token.IsCancellationRequested)
        //     {
        //         _logger.LogInformation("Worker running at: {Time}", DateTimeOffset.Now);
        //         await Task.Delay(1000, cts.Token);
        //     }
        // }
        // catch (TaskCanceledException)
        // {
        //     _hostApplicationLifetime.StopApplication();
        // }
        using var scope = _serviceProvider.CreateScope();
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IAppDbContextInitializer>();
        try
        {
            await dbInitializer.InitializeAsync(stoppingToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during database migration");
            throw;
        }
        finally
        {
            _hostApplicationLifetime.StopApplication();
        }
    }
}