using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SplitTheBill.Domain.Models.Groups;

namespace SplitTheBill.Persistence.Configuration;

internal sealed class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.ToTable("Expenses");
        
        builder
            .HasOne<Group>()
            .WithMany(g => g.Expenses)
            .HasForeignKey(e => e.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(e => e.Participants)
            .WithOne()
            .HasForeignKey(ep => ep.ExpenseId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasOne(e => e.PaidByMember)
            .WithMany()
            .HasForeignKey(e => e.PaidByMemberId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}