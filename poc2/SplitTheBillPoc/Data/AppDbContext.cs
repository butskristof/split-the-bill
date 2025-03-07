using Microsoft.EntityFrameworkCore;

namespace SplitTheBillPoc.Data;

internal sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
}