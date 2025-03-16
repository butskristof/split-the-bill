using Microsoft.EntityFrameworkCore;
using SplitTheBill.Application.Common.Constants;
using SplitTheBill.Application.Common.Persistence;
using SplitTheBill.Domain.Models.Groups;
using SplitTheBill.Domain.Models.Members;

namespace SplitTheBill.Persistence;

internal sealed class AppDbContext : DbContext, IAppDbContext
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
        // the base method is empty, but retain the call to minimise impact if
        // it should be used in a future version
        base.ConfigureConventions(configurationBuilder);

        // set text fields to have a reduced maximum length by default 
        // this cuts down on a lot of varchar(max) columns, and can still be set to a higher 
        // maximum length on a per-column basis
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
        // the base method is empty, but retain the call to minimise impact if
        // it should be used in a future version
        base.OnModelCreating(modelBuilder);

        // modelBuilder.HasCollation(
        //     PersistenceConstants.CaseInsensitiveCollation,
        //     locale: "en-u-ks-primary",
        //     provider: "icu",
        //     deterministic: false
        // );
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    #endregion
}