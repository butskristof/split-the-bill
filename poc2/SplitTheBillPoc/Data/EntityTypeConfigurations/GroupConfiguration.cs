using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SplitTheBillPoc.Models;

namespace SplitTheBillPoc.Data.EntityTypeConfigurations;

internal sealed class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder
            .HasMany<Member>(g => g.Members)
            .WithMany(m => m.Groups)
            .UsingEntity<GroupMember>();
    }
}