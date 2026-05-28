using Microsoft.AspNetCore.Mvc;

namespace Api.Middlewares;

public class ExceptionMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context)
    {
        var traceId = Guid.NewGuid();
        try
        {
            // This should probably be a logger...
            Console.WriteLine(
                $"Method {context.Request.Method} on path {context.Request.Path} called"
            );
            await _next(context);
        }
        catch (UnauthorizedAccessException ex)
        {
            var problemDetails = new ProblemDetails
            {
                Title = "Unauthorized",
                Status = StatusCodes.Status403Forbidden,
                Instance = context.Request.Path,
                Detail = $"{ex.Message} traceId: {traceId}",
            };

            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
        catch (KeyNotFoundException ex)
        {
            var problemDetails = new ProblemDetails
            {
                Title = "Resource Not Found",
                Status = StatusCodes.Status404NotFound,
                Instance = context.Request.Path,
                Detail = $"{ex.Message} traceId: {traceId}",
            };

            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            var problemDetails = new ProblemDetails
            {
                Title = "Internal Server Error",
                Status = StatusCodes.Status500InternalServerError,
                Instance = context.Request.Path,
                Detail = $"Internal server error occured, traceId : {traceId}",
            };
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
