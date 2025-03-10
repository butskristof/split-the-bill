using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SplitTheBillPocV4.Models;

namespace SplitTheBillPocV4.Data.EntityTypeConfigurations;

internal sealed class ExpenseParticipantConfiguration : IEntityTypeConfiguration<ExpenseParticipant>
{
    public void Configure(EntityTypeBuilder<ExpenseParticipant> builder)
    {
        builder.ToTable("ExpenseParticipants");

        builder
            .HasOne<Expense>()
            .WithMany()
            .HasForeignKey(ep => ep.ExpenseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne<Member>()
            .WithMany()
            .HasForeignKey(ep => ep.MemberId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}