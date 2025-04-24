using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SplitTheBill.Domain.Models.Groups;
using SplitTheBill.Domain.Models.Members;

namespace SplitTheBill.Persistence.Configuration;

internal sealed class GroupMemberConfiguration : IEntityTypeConfiguration<GroupMember>
{
    public void Configure(EntityTypeBuilder<GroupMember> builder)
    {
        builder.ToTable("GroupMembers");

        builder
            .HasOne<Group>()
            .WithMany(g => g.GroupMembers)
            .HasForeignKey(gm => gm.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne<Member>()
            .WithMany()
            .HasForeignKey(gm => gm.MemberId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
