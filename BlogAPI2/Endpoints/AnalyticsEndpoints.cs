using BlogAPI2.Contracts;
using BlogAPI2.Database;
using BlogAPI2.Entities;
using BlogAPI2.Helpers;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI2.Endpoints
{
    public static class AnalyticsEndpoints
    {
        public static void MapAnalyticsEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("visitors", async (Visitor request, ApplicationDbContext context, CancellationToken ct) =>
            {
                var ipAddress = RequestHelper.GetIpAddress();

                var visitor = new Visitor
                {
                    Id = new Guid(),
                    VisitorIp = ipAddress,
                    ScreenHeight = request.ScreenHeight,
                    ScreenWidth = request.ScreenWidth,
                    DateVisited = DateTime.UtcNow,
                };

                context.Add(visitor);

                await context.SaveChangesAsync(ct);

                return Results.Ok(visitor);
            });

            app.MapPut("visitors", async (UpdateVisitorRequest request, ApplicationDbContext context, CancellationToken ct) =>
            {
                var updatedVisitor = new Visitor { Id = request.Id };
                context.Visitors.Attach(updatedVisitor);

                if (request.ViewedBlogs)
                {
                    updatedVisitor.ViewedBlogs = true;
                    context.Entry(updatedVisitor).Property(x => x.ViewedBlogs).IsModified = true;
                }

                if (request.ViewedProjects)
                {
                    updatedVisitor.ViewedProjects = true;
                    context.Entry(updatedVisitor).Property(x => x.ViewedProjects).IsModified = true;
                }

                if (request.ViewedAbout)
                {
                    updatedVisitor.ViewedAbout = true;
                    context.Entry(updatedVisitor).Property(x => x.ViewedAbout).IsModified = true;
                }

                return await context.SaveChangesAsync();
            });

            app.MapGet("visitorsCount", (ApplicationDbContext context) =>
            {
                var totalVisits = context.Visitors.Count();

                return Results.Ok(totalVisits);
            });

            app.MapGet("visitors", async (ApplicationDbContext context, CancellationToken ct, int page = 1, int pageSize = 10) =>
            {
                var visitors = await context.Visitors
                    .AsNoTracking()
                    .OrderByDescending(x => x.DateVisited)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(ct);

                return Results.Ok(visitors);
            });
        }
    }
}
