using Microsoft.EntityFrameworkCore;
using SplitTheBillPocV4.Models;

namespace SplitTheBillPocV4.Data;

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
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder
            .Properties<string>()
            .HaveMaxLength(512);
    }
}