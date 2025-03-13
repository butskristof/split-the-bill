using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SplitTheBill.Domain.Models.Groups;
using SplitTheBill.Domain.Models.Members;

namespace SplitTheBill.Persistence.Configuration;

internal sealed class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder
            .HasMany(m => m.Groups)
            .WithMany(g => g.Members)
            .UsingEntity<GroupMember>();
    }
}