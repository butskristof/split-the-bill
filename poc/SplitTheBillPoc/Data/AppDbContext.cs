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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Group>()
            .HasMany<Member>(g => g.Members)
            .WithMany();
    }
}