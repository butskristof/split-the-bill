using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SplitTheBill.Domain.Models.Groups;

namespace SplitTheBill.Persistence.Configuration;

internal sealed class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder
            .HasMany(g => g.Members)
            .WithMany(m => m.Groups)
            .UsingEntity<GroupMember>();
    }
}