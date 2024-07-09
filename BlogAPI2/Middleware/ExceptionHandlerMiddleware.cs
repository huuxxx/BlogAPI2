using BlogAPI2.Database;
using BlogAPI2.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI2.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _next = next;
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);
                await LogExceptionToDatabaseAsync(exception);

                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Server Error"
                };

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(problemDetails);
            }
        }

        private async Task LogExceptionToDatabaseAsync(Exception exception)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var exceptionInfo = new ExceptionInfo
                {
                    Id = new Guid(),
                    DateCreated = DateTime.UtcNow,
                    StackTrace = exception.StackTrace,
                    Message = exception.Message
                };

                context.Add(exceptionInfo);
                await context.SaveChangesAsync();
            }
        }
    }
}
