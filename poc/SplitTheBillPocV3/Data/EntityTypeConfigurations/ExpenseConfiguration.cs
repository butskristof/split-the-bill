using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SplitTheBillPocV3.Models;

namespace SplitTheBillPocV3.Data.EntityTypeConfigurations;

internal sealed class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder
            .HasMany<Member>(e => e.Participants)
            .WithMany()
            .UsingEntity<ExpenseParticipant>();
    }
}