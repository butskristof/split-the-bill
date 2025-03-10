using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SplitTheBillPocV2.Models;

namespace SplitTheBillPocV2.Data.EntityTypeConfigurations;

internal sealed class GroupMemberConfiguration : IEntityTypeConfiguration<GroupMember>
{
    public void Configure(EntityTypeBuilder<GroupMember> builder)
    {
        builder.ToTable("GroupMembers");

        builder
            .HasOne<Group>()
            .WithMany()
            .HasForeignKey(gm => gm.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne<Member>()
            .WithMany()
            .HasForeignKey(gm => gm.MemberId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}