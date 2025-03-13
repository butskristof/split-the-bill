using Microsoft.EntityFrameworkCore;
using SplitTheBill.Application.Common.Constants;
using SplitTheBill.Domain.Models.Groups;
using SplitTheBill.Domain.Models.Members;

namespace SplitTheBill.Persistence;

internal sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    #region Entities

    public DbSet<Member> Members { get; set; }
    public DbSet<Group> Groups { get; set; }

    #endregion

    #region Configuration

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder
            .Properties<string>()
            .HaveMaxLength(ApplicationConstants.DefaultMaxStringLength);

        configurationBuilder
            .Properties<decimal>()
            .HavePrecision(18, 6);
        
        // TODO DateTimeOffset
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    #endregion
}