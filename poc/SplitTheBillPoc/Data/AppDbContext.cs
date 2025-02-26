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
    }
}