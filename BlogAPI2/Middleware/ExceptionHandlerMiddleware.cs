using BlogAPI2.Database;
using BlogAPI2.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI2.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        private readonly ApplicationDbContext _context;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger, ApplicationDbContext context)
        {
            _next = next;
            _logger = logger;
            _context = context;
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

                var exceptionInfo = new ExceptionInfo
                {
                    Id = new Guid(),
                    DateCreated = DateTime.UtcNow,
                    StackTrace = exception.StackTrace,
                    Message = exception.Message
                };

                _context.Add(exceptionInfo);
                _context.SaveChanges();

                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Server Error"
                };

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(problemDetails);
            }
        }
    }
}
