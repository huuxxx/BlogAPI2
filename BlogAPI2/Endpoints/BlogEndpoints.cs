using Microsoft.EntityFrameworkCore;
using BlogAPI2.Contracts;
using BlogAPI2.Database;
using BlogAPI2.Entities;
using Microsoft.AspNetCore.Mvc;
using BlogAPI2.Helpers;

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
                var products = await context.Blogs
                    .AsNoTracking()
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(ct);

                return Results.Ok(products);
            });

            app.MapGet("blogs/{id}", async (Guid id, ApplicationDbContext context, CancellationToken ct) =>
            {
                var blog = await context.Blogs.FirstOrDefaultAsync(p => p.Id == id, ct);

                return blog is null ? Results.NotFound() : Results.Ok(blog);
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

            app.MapPost("images", async ([FromForm] IFormFile file, CancellationToken ct) =>
            {
                string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                string path = Path.Combine(Directory.GetCurrentDirectory(), @$"{ConfigurationHelper.GetImagesDirectory}\" + timeStamp + Path.GetExtension(file.FileName));
                
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream, ct);
                }

                return $"{ConfigurationHelper.GetApiUrl}/{ConfigurationHelper.GetImagesDirectory}/{timeStamp + Path.GetExtension(file.FileName)}";
            });

            app.MapDelete("images/{id}", (string id) =>
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), $@"{ConfigurationHelper.GetImagesDirectory}\" + id);
                File.Delete(path);
                return Results.NoContent();
            });

            app.MapGet("images", () =>
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), $"{ConfigurationHelper.GetImagesDirectory}");
                string[] images = Directory.GetFiles(path);

                if (images.Length > 0)
                {
                    for (int i = 0; i < images.Length; i++)
                    {
                        images[i] = Path.GetFileName(images[i]);
                    }
                }

                return Results.Ok(images);
            });
        }
    }
}
