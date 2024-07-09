using Microsoft.EntityFrameworkCore;
using BlogAPI2.Contracts;
using BlogAPI2.Database;
using BlogAPI2.Entities;

namespace BlogAPI2.Endpoints
{
    public static class BlogEndpoints
    {
        public static void MapBlogEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("blogs", async (CreateBlogRequest request, ApplicationDbContext context, CancellationToken ct) =>
            {
                var blog = new Blog
                {
                    Id = new Guid(),
                    Title = request.Title,
                    Content = request.Content,
                    DateCreated = DateTime.UtcNow,
                };

                context.Add(blog);

                await context.SaveChangesAsync(ct);

                return Results.Ok(blog);
            });

            app.MapGet("blogs", async (ApplicationDbContext context, CancellationToken ct, int page = 1, int pageSize = 10) =>
            {
                var blogs = await context.Blogs
                    .AsNoTracking()
                    .OrderBy(x => x.DateCreated)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(ct);

                return Results.Ok(blogs);
            });

            app.MapGet("blogs/{id}", async (Guid id, ApplicationDbContext context, CancellationToken ct) =>
            {
                var blog = await context.Blogs.FirstOrDefaultAsync(p => p.Id == id, ct);

                if (blog != null)
                {
                    var increment = blog.ViewCount + 1;
                    blog.ViewCount = increment;
                    await context.SaveChangesAsync(ct);
                    return Results.Ok(blog);
                }

                return Results.NotFound();
            });

            app.MapGet("blogIds", async (ApplicationDbContext context, CancellationToken ct) =>
            {
                var blogs = await context.Blogs.OrderBy(x => x.DateCreated).Select(x => x.Id).ToListAsync(ct);

                return blogs is null ? Results.NotFound() : Results.Ok(blogs);
            });

            app.MapPut("blogs/{id}", async (Guid id, UpdateBlogRequest request, ApplicationDbContext context, CancellationToken ct) =>
            {
                var blog = await context.Blogs
                    .FirstOrDefaultAsync(p => p.Id == id, ct);

                if (blog is null)
                {
                    return Results.NotFound();
                }

                blog.Title = request.Title;
                blog.Content = request.Content;
                blog.DateModified = DateTime.UtcNow;

                await context.SaveChangesAsync(ct);

                return Results.NoContent();
            });

            app.MapDelete("blogs/{id}", async (Guid id, ApplicationDbContext context, CancellationToken ct) =>
            {
                var blog = await context.Blogs
                    .FirstOrDefaultAsync(p => p.Id == id, ct);

                if (blog is null)
                {
                    return Results.NotFound();
                }

                context.Remove(blog);

                await context.SaveChangesAsync(ct);

                return Results.NoContent();
            });
        }
    }
}
