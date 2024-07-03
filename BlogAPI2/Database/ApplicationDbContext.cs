using Microsoft.EntityFrameworkCore;
using BlogAPI2.Entities;

namespace BlogAPI2.Database;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Blog> Blogs { get; set; }
}