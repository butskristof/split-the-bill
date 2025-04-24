using System.Security.Authentication;
using Microsoft.EntityFrameworkCore;
using SplitTheBill.Application.Common.Authentication;
using SplitTheBill.Application.Common.Constants;
using SplitTheBill.Application.Common.Persistence;
using SplitTheBill.Domain.Models.Groups;
using SplitTheBill.Domain.Models.Members;
using SplitTheBill.Persistence.Common;
using SplitTheBill.Persistence.ValueConverters;

namespace SplitTheBill.Persistence;

internal sealed class AppDbContext : DbContext, IAppDbContext
{
    #region construction

    private readonly IAuthenticationInfo _authenticationInfo;

    public AppDbContext(DbContextOptions options, IAuthenticationInfo authenticationInfo)
        : base(options)
    {
        _authenticationInfo = authenticationInfo;
    }

    #endregion

    #region Entities

    public DbSet<Member> Members { get; set; }
    public DbSet<Group> Groups { get; set; }

    #endregion

    public IQueryable<Group> CurrentUserGroups(bool tracking)
    {
        var query = Groups.Where(g => g.Members.Any(m => m.UserId == _authenticationInfo.UserId));

        if (!tracking)
            query = query.AsNoTracking();

        return query;
    }

    public async Task<Member> GetMemberForCurrentUserAsync(
        bool tracking = false,
        CancellationToken cancellationToken = default
    )
    {
        var query = Members.Where(m => m.UserId == _authenticationInfo.UserId);
        if (!tracking)
            query = query.AsNoTracking();

        return await query.SingleOrDefaultAsync(cancellationToken)
            ?? throw new AuthenticationException(
                "Could not find Member for currently authenticated user"
            );
    }

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

        configurationBuilder.Properties<decimal>().HavePrecision(18, 6);

        configurationBuilder
            .Properties<DateTimeOffset>()
            .HaveConversion<DateTimeOffsetValueConverter>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // the base method is empty, but retain the call to minimise impact if
        // it should be used in a future version
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasCollation(
            PersistenceConstants.CaseInsensitiveCollation,
            locale: "en-u-ks-primary",
            provider: "icu",
            deterministic: false
        );
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    #endregion
}
