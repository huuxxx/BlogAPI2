using Microsoft.EntityFrameworkCore;
using BlogAPI2.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BlogAPI2.Database;

public sealed class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Visitor> Visitors { get; set; }
}