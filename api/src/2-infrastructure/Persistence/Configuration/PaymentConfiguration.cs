using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SplitTheBill.Domain.Models.Groups;
using SplitTheBill.Domain.Models.Members;

namespace SplitTheBill.Persistence.Configuration;

internal sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");
        
        builder
            .HasOne<Group>()
            .WithMany(g => g.Payments)
            .HasForeignKey(p => p.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne<Member>()
            .WithMany()
            .HasForeignKey(p => p.SendingMemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<Member>()
            .WithMany()
            .HasForeignKey(p => p.ReceivingMemberId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}