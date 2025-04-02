using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SplitTheBill.Application.Common.Constants;
using SplitTheBill.Domain.Models.Groups;
using SplitTheBill.Domain.Models.Members;
using SplitTheBill.Persistence.Common;

namespace SplitTheBill.Persistence.Configuration;

internal sealed class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.ToTable("Members");

        builder.Property(m => m.Username)
            .UseCollation(PersistenceConstants.CaseInsensitiveCollation)
            .HasMaxLength(ApplicationConstants.MemberUsernameMaxLength);
        builder.HasIndex(m => m.Username)
            .IsUnique();

        builder.Property(m => m.UserId)
            .UseCollation(PersistenceConstants.CaseInsensitiveCollation);
        builder.HasIndex(m => m.UserId)
            .IsUnique();
        
        builder
            .HasMany(m => m.Groups)
            .WithMany(g => g.Members)
            .UsingEntity<GroupMember>();
    }
}