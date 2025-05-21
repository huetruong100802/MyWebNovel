using MyWebNovel.Application.Exceptions;

namespace MyWebNovel.Api.Middleware
{
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Call the next middleware in the pipeline
                await next(context);
            }
            catch (NotFoundException ex)
            {
                // Handle NotFoundException (e.g., return 404)
                //logger.LogWarning(ex, "Resource not found: {Message}", ex.Message);
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsJsonAsync(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                // Handle UnauthorizedAccessException (e.g., return 401)
                logger.LogWarning(ex, "Unauthorized access: {Message}", ex.Message);
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { error = "Unauthorized access." });
            }
            catch (Exception ex)
            {
                // Handle all other exceptions (e.g., return 500)
                logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new { error = "An unexpected error occurred. Please try again later." });
            }
        }
    }

}
