using Microsoft.EntityFrameworkCore;
using BlogAPI2.Contracts;
using BlogAPI2.Database;
using BlogAPI2.Entities;

namespace BlogAPI2.Endpoints
{
    public static class BlogEndpoints
    {
        public static void MapProductEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("blogs", async (CreateBlogRequest request, ApplicationDbContext context, CancellationToken ct) =>
            {
                var blog = new Blog
                {
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
                var products = await context.Blogs
                    .AsNoTracking()
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(ct);

                return Results.Ok(products);
            });

            app.MapGet("blogs/{id}", async (int id, ApplicationDbContext context, CancellationToken ct) =>
            {
                var blog = await context.Blogs.FirstOrDefaultAsync(p => p.Id == id, ct);

                return blog is null ? Results.NotFound() : Results.Ok(blog);
            });

            app.MapPut("blogs/{id}", async (int id, UpdateBlogRequest request, ApplicationDbContext context, CancellationToken ct) =>
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

            app.MapDelete("blogs/{id}", async (int id, ApplicationDbContext context, CancellationToken ct) =>
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
