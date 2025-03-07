using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SplitTheBillPoc.Models;

namespace SplitTheBillPoc.Data.EntityTypeConfigurations;

internal sealed class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder
            .HasMany<Group>(m => m.Groups)
            .WithMany(g => g.Members)
            .UsingEntity<GroupMember>();
    }
}