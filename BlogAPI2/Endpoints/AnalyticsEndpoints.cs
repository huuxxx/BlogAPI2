using BlogAPI2.Contracts;
using BlogAPI2.Database;
using BlogAPI2.DTO;
using BlogAPI2.Entities;
using BlogAPI2.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace BlogAPI2.Endpoints
{
    public static class AnalyticsEndpoints
    {
        public static void MapAnalyticsEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("visitors", async (Visitor request, ApplicationDbContext context, CancellationToken ct, RequestHelper requestHelper) =>
            {
                var ipAddress = requestHelper.GetIpAddress();

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
                var visitor = await context.Visitors.FirstOrDefaultAsync(x => x.Id == request.Id, ct);

                if (visitor is null)
                {
                    return Results.NotFound();
                }

                context.Visitors.Attach(visitor);

                if (request.ViewedBlogs)
                {
                    visitor.ViewedBlogs = true;
                    context.Entry(visitor).Property(x => x.ViewedBlogs).IsModified = true;
                }

                if (request.ViewedProjects)
                {
                    visitor.ViewedProjects = true;
                    context.Entry(visitor).Property(x => x.ViewedProjects).IsModified = true;
                }

                if (request.ViewedAbout)
                {
                    visitor.ViewedAbout = true;
                    context.Entry(visitor).Property(x => x.ViewedAbout).IsModified = true;
                }

                await context.SaveChangesAsync(ct);

                return Results.Ok();
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

            app.MapGet("visitorsPastWeek", (ApplicationDbContext context) =>
            {
                var oneWeekAgo = DateTime.UtcNow.AddDays(-6).Date;

                var query = context.Visitors
                    .Where(v => v.DateVisited >= oneWeekAgo)
                    .GroupBy(v => v.DateVisited.Date)
                    .Select(g => new
                    {
                        Date = g.Key,
                        Count = g.Count()
                    })
                    .ToList();

                var result = Enumerable.Range(0, 7)
                    .Select(i => oneWeekAgo.AddDays(i))
                    .GroupJoin(query,
                               date => date,
                               item => item.Date,
                               (date, items) => new VisitorsPastWeekDto
                               {
                                   VisitsInDay = items.FirstOrDefault()?.Count ?? 0,
                                   NameOfDay = date.ToString("dddd", CultureInfo.InvariantCulture)
                               })
                    .ToList();

                return Results.Ok(result);
            });
        }
    }
}
