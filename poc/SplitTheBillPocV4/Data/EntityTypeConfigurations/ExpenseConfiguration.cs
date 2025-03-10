using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SplitTheBillPocV4.Models;

namespace SplitTheBillPocV4.Data.EntityTypeConfigurations;

internal sealed class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder
            .HasMany<ExpenseParticipant>(e => e.Participants)
            .WithOne()
            .HasForeignKey(ep => ep.ExpenseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}