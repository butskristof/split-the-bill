using Microsoft.EntityFrameworkCore;
using SplitTheBill.Domain.Models.Groups;
using SplitTheBill.Domain.Models.Members;

namespace SplitTheBill.Application.Common.Persistence;

public interface IAppDbContext
{
    DbSet<Member> Members { get; }
    DbSet<Group> Groups { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}