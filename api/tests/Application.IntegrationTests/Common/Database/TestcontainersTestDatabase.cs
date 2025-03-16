using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Respawn;
using Respawn.Graph;
using SplitTheBill.Persistence;
using Testcontainers.PostgreSql;

namespace SplitTheBill.Application.IntegrationTests.Common.Database;

// this implementation uses the Testcontainers library to spin up a Postgres Docker container,
// prepare the database and dispose of it afterwards
// it assures a self-contained process against a production-grade database, but can suffer from 
// longer startup times since the container has to be prepared before each test run

internal sealed class TestcontainersTestDatabase : ITestDatabase
{
    private readonly PostgreSqlContainer _container;
    private string _connectionString = null!;
    private DbConnection _dbConnection = null!;
    private Respawner _respawner = null!;

    public TestcontainersTestDatabase()
    {
        _container = new PostgreSqlBuilder()
            .WithAutoRemove(true)
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        _connectionString = _container.GetConnectionString();
        _dbConnection = new NpgsqlConnection(_connectionString);
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(_connectionString, Persistence.DependencyInjection.GetDbContextOptionsBuilder())
            .Options;
        var context = new AppDbContext(options);
        context.Database.Migrate();

        await using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            _respawner = await Respawner.CreateAsync(connection, new RespawnerOptions
            {
                TablesToIgnore = new Table[] { "__EFMigrationsHistory" },
                DbAdapter = DbAdapter.Postgres,
            });
        }
    }

    public DbConnection GetConnection() => _dbConnection;

    public async Task ResetAsync()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await _respawner.ResetAsync(connection);
    }

    public async Task DisposeAsync()
    {
        await _dbConnection.DisposeAsync();
        await _container.DisposeAsync();
    }
}