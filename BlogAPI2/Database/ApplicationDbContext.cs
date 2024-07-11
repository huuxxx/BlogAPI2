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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BlogTag>()
            .HasKey(bt => new { bt.BlogId, bt.TagId });

        modelBuilder.Entity<BlogTag>()
            .HasOne(bt => bt.Blog)
            .WithMany(b => b.BlogTags)
            .HasForeignKey(bt => bt.BlogId);

        modelBuilder.Entity<BlogTag>()
            .HasOne(bt => bt.Tag)
            .WithMany(t => t.BlogTags)
            .HasForeignKey(bt => bt.TagId);
    }

    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Visitor> Visitors { get; set; }
    public DbSet<ExceptionInfo> ExceptionInfo { get; set; }
    public DbSet<Tag> Tag { get; set; }
    public DbSet<BlogTag> BlogTag { get; set; }
}