using Microsoft.EntityFrameworkCore;
using BlogAPI2.Contracts;
using BlogAPI2.Database;
using BlogAPI2.Entities;
using BlogAPI2.Helpers;

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
                    BlogTags = new List<BlogTag>(),
                };

                foreach (var tagName in request.Tags)
                {
                    if (string.IsNullOrEmpty(tagName)) continue;

                    var tag = await context.Tag.FirstOrDefaultAsync(t => t.Name == tagName);

                    if (tag is null)
                    {
                        tag = new Tag { Name = tagName };
                        context.Tag.Add(tag);
                    }

                    blog.BlogTags.Add(new BlogTag { Blog = blog, Tag = tag });
                }

                context.Add(blog);
                await context.SaveChangesAsync(ct);

                return Results.Ok();
            })
            .RequireAuthorization();

            app.MapGet("blogs", async (ApplicationDbContext context, CancellationToken ct, int page = 1, int pageSize = 10) =>
            {
                var blogs = await context.Blogs
                    .OrderByDescending(b => b.DateCreated)
                    .Include(b => b.BlogTags)
                    .ThenInclude(bt => bt.Tag)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(ct);

                var response = MappingHelper.BlogEntityToDto(blogs);

                return Results.Ok(response);
            });

            app.MapGet("blogs/{id}", async (Guid id, ApplicationDbContext context, CancellationToken ct, bool? increment = true) =>
            {
                var blog = await context.Blogs
                    .Include(b => b.BlogTags)
                    .ThenInclude(bt => bt.Tag)
                    .FirstOrDefaultAsync(b => b.Id == id, ct);

                if (blog is null)
                {
                    return Results.NotFound();
                }

                if (increment is true)
                {
                    var incrementValue = blog.ViewCount + 1;
                    blog.ViewCount = incrementValue;
                    await context.SaveChangesAsync(ct);
                    
                }
                var response = MappingHelper.BlogEntityToDto(blog);
                return Results.Ok(response);
            });

            app.MapGet("blogIds", async (ApplicationDbContext context, CancellationToken ct) =>
            {
                var blogIds = await context.Blogs.OrderBy(x => x.DateCreated).Select(x => x.Id).ToListAsync(ct);
                return blogIds is null ? Results.NotFound() : Results.Ok(blogIds);
            });

            app.MapPut("blogs", async (UpdateBlogRequest request, ApplicationDbContext context, CancellationToken ct) =>
            {
                var blog = await context.Blogs
                    .Include(b => b.BlogTags)
                    .ThenInclude(bt => bt.Tag)
                    .FirstOrDefaultAsync(b => b.Id == request.Id, ct);

                if (blog is null)
                {
                    return Results.BadRequest();
                }

                blog.Title = request.Title;
                blog.Content = request.Content;
                blog.DateModified = DateTime.UtcNow;

                var existingTags = blog.BlogTags.Select(bt => bt.Tag.Name).ToList();

                var tagsToAdd = request.Tags.Except(existingTags).ToList();
                var tagsToRemove = existingTags.Except(request.Tags).ToList();

                foreach (var tagName in tagsToAdd)
                {
                    var tag = await context.Tag.FirstOrDefaultAsync(t => t.Name == tagName);

                    if (tag is null)
                    {
                        tag = new Tag { Name = tagName };
                        context.Tag.Add(tag);
                    }

                    blog.BlogTags.Add(new BlogTag { Blog = blog, Tag = tag });
                }

                foreach (var tagName in tagsToRemove)
                {
                    var blogTag = blog.BlogTags.FirstOrDefault(bt => bt.Tag.Name == tagName);

                    if (blogTag is not null)
                    {
                        blog.BlogTags.Remove(blogTag);
                        context.BlogTag.Remove(blogTag);
                    }
                }

                await context.SaveChangesAsync(ct);

                return Results.Ok();
            })
            .RequireAuthorization();


            app.MapDelete("blogs/{id}", async (Guid id, ApplicationDbContext context, CancellationToken ct) =>
            {
                var blog = await context.Blogs.FirstOrDefaultAsync(x => x.Id == id, ct);

                if (blog is null)
                {
                    return Results.NotFound();
                }

                context.Remove(blog);
                await context.SaveChangesAsync(ct);

                return Results.Ok();
            })
            .RequireAuthorization();

            app.MapPost("tags/{name}", async (string name, ApplicationDbContext context, CancellationToken ct) =>
            {
                if (string.IsNullOrEmpty(name))
                {
                    return Results.BadRequest();
                }

                var tagCheck = await context.Tag.FirstOrDefaultAsync(x => x.Name == name, ct);

                if (tagCheck is not null)
                {
                    return Results.BadRequest();
                }

                var Tag = new Tag
                {
                    Id = new Guid(),
                    Name = name,
                };

                context.Tag.Add(Tag);
                await context.SaveChangesAsync(ct);

                return Results.Ok();
            })
            .RequireAuthorization();

            app.MapGet("tags", async (ApplicationDbContext context, CancellationToken ct) =>
            {
                var tags = await context.Tag.OrderBy(t => t.Name).ToListAsync(ct);
                var response = tags.Select(t => t.Name).ToArray();

                return Results.Ok(response);
            });

            app.MapGet("blogsByTag", async (string[] tags, ApplicationDbContext context, CancellationToken ct, int page = 1, int pageSize = 10) =>
            {
                var blogs = await context.Blogs
                    .Where(b => b.BlogTags.Any(bt => tags.Contains(bt.Tag.Name)))
                    .OrderBy(b => b.DateCreated)
                    .Include(b => b.BlogTags)
                    .ThenInclude(bt => bt.Tag)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(ct);

                var response = MappingHelper.BlogEntityToDto(blogs);

                return Results.Ok(response);
            });

            app.MapDelete("tags/{name}", async (string name, ApplicationDbContext context, CancellationToken ct) =>
            {
                var tag = await context.Tag
                    .Include(t => t.BlogTags)
                    .FirstOrDefaultAsync(t => t.Name == name, ct);

                if (tag is null)
                {
                    return Results.BadRequest();
                }

                context.BlogTag.RemoveRange(tag.BlogTags);
                context.Tag.Remove(tag);
                await context.SaveChangesAsync(ct);

                return Results.Ok();
            })
            .RequireAuthorization();
        }
    }
}
