using BlogAPI2.Database;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI2.Endpoints
{
    public static class ExceptionEndpoints
    {
        public static void MapExceptionEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("exceptions", async (ApplicationDbContext context, CancellationToken ct) =>
            {
                var exceptions = await context.ExceptionInfo.OrderBy(x => x.DateCreated).ToListAsync(ct);

                return Results.Ok(exceptions);
            }).RequireAuthorization();

            app.MapDelete("exceptions", async (ApplicationDbContext context, CancellationToken ct) =>
            {
                var exceptions = await context.ExceptionInfo.ToListAsync(ct);

                if (exceptions.Count == 0)
                {
                    return Results.NotFound();
                }

                context.RemoveRange(exceptions);

                await context.SaveChangesAsync(ct);

                return Results.NoContent();
            }).RequireAuthorization();

        }
    }
}
