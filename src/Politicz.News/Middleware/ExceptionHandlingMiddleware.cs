namespace Politicz.News.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context, ILogger<ExceptionHandlingMiddleware> logger)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new Failure("Internal Server Error."));
            logger.LogError(exception, "Unexpected error");
        }
    }
}
