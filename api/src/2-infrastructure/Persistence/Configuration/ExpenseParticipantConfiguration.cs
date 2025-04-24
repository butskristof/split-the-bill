using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SplitTheBill.Domain.Models.Groups;

namespace SplitTheBill.Persistence.Configuration;

internal sealed class ExpenseParticipantConfiguration : IEntityTypeConfiguration<ExpenseParticipant>
{
    public void Configure(EntityTypeBuilder<ExpenseParticipant> builder)
    {
        builder.ToTable("ExpenseParticipants");

        builder.HasKey(ep => new { ep.ExpenseId, ep.MemberId });

        builder
            .HasOne<Expense>()
            .WithMany(e => e.Participants)
            .HasForeignKey(ep => ep.ExpenseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
