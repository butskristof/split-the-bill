using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SplitTheBillPocV2.Models;

namespace SplitTheBillPocV2.Data.EntityTypeConfigurations;

internal sealed class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder
            .HasMany<Group>(m => m.Groups)
            .WithMany(g => g.Members)
            .UsingEntity<GroupMember>();

        builder
            .HasMany<Expense>()
            .WithMany()
            .UsingEntity<ExpenseParticipant>();
    }
}