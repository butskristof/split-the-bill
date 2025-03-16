using System.Linq.Expressions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Time.Testing;
using SplitTheBill.Application.IntegrationTests.Common.Database;
using SplitTheBill.Persistence;
using TUnit.Core.Interfaces;

namespace SplitTheBill.Application.IntegrationTests.Common;

// this collection fixture sets up a database and builds a minimal, but representative, service collection
// so the application layer can be configured, with a scope factory as a result
// it also provides various utility methods to access the underlying database and authentication 
// so integration tests can be easily "arranged" before "acting" by sending requests into the mediator

internal sealed class ApplicationFixture : IAsyncInitializer, IAsyncDisposable
{
    private ITestDatabase _database = null!;
    private FakeTimeProvider _timeProvider = new();

    private IServiceScopeFactory _scopeFactory = null!;

    public async Task InitializeAsync()
    {
        _database = await TestDatabaseFactory.CreateAsync();

        // build a service collection
        var services = new ServiceCollection();
        // start by adding the application layer
        services
            .AddApplication();

        // add in required ports & adapters by replacing them with fakes
        // infrastructure
        services
            .AddLogging()
            .AddScoped<TimeProvider>(_ => _timeProvider);

        // persistence
        services
            .AddPersistence(null, _database.GetConnection());

        // build a scope factory from the service collection
        var provider = services.BuildServiceProvider();
        _scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();
    }

    public async Task ResetStateAsync()
    {
        await _database.ResetAsync();
        _timeProvider = new FakeTimeProvider();
    }

    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();
        return await sender.Send(request);
    }

    public async Task SendAsync(IBaseRequest request)
    {
        using var scope = _scopeFactory.CreateScope();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();
        await sender.Send(request);
    }

    public async Task<TEntity?> FindAsync<TEntity>(params object[] keyValues)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        return await context.FindAsync<TEntity>(keyValues);
    }

    public async Task<TEntity?> FindAsync<TEntity>(
        Expression<Func<TEntity, bool>> identifier,
        params Expression<Func<TEntity, object>>[] includes)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var query = context.Set<TEntity>().AsQueryable();
        query = includes.Aggregate(query, (current, include) => current.Include(include));
        return await query.SingleOrDefaultAsync(identifier);
    }

    public async Task AddAsync<TEntity>(params TEntity[] entities)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        context.AddRange(entities);
        await context.SaveChangesAsync();
    }

    public async Task<int> CountAsync<TEntity>()
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        return await context.Set<TEntity>().CountAsync();
    }


    public async ValueTask DisposeAsync()
    {
        await _database.DisposeAsync();
    }
}