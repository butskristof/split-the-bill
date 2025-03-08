using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SplitTheBillPocV2.Models;

namespace SplitTheBillPocV2.Data.EntityTypeConfigurations;

internal sealed class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder
            .HasMany<Member>(g => g.Members)
            .WithMany(m => m.Groups)
            .UsingEntity<GroupMember>();

        builder
            .HasMany<Expense>(g => g.Expenses)
            .WithOne()
            .HasForeignKey(e => e.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany<Payment>(g => g.Payments)
            .WithOne()
            .HasForeignKey(p => p.GroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}