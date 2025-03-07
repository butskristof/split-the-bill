using Microsoft.EntityFrameworkCore;
using SplitTheBillPoc.Models;

namespace SplitTheBillPoc.Data;

internal sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Member> Members { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<Payment> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Group>()
            .HasMany<Member>(g => g.Members)
            .WithMany();

        modelBuilder
            .Entity<Expense>()
            .HasOne<Group>()
            .WithMany(g => g.Expenses)
            .HasForeignKey(e => e.GroupId);

        modelBuilder
            .Entity<Expense>()
            .HasOne<Member>()
            .WithMany()
            .HasForeignKey(e => e.PaidByMemberId);

        modelBuilder
            .Entity<Payment>()
            .HasOne<Group>()
            .WithMany(g => g.Payments)
            .HasForeignKey(p => p.GroupId);

        modelBuilder
            .Entity<Payment>()
            .HasOne<Member>()
            .WithMany()
            .HasForeignKey(p => p.PaidByMemberId);

        modelBuilder
            .Entity<Payment>()
            .HasOne<Member>()
            .WithMany()
            .HasForeignKey(p => p.PaidToMemberId);
    }
}