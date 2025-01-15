using Microsoft.EntityFrameworkCore;
using TraceLink.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<TagDetails> TagDetails { get; set; }
}
