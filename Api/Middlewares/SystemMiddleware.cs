using Microsoft.AspNetCore.Mvc;

namespace Api.Middlewares;

public class SystemMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        var traceId = Guid.NewGuid();
        try
        {
            // This will be logging...
            Console.WriteLine(
                $"Method {context.Request.Method} on path {context.Request.Path} called"
            );
            await next(context);
        }
        catch (UnauthorizedAccessException ex)
        {
            var problemDetails = new ProblemDetails
            {
                Title = "Unauthorized",
                Status = StatusCodes.Status401Unauthorized,
                Instance = context.Request.Path,
                Detail = ex.Message,
            };

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
        catch (KeyNotFoundException ex)
        {
            var problemDetails = new ProblemDetails
            {
                Title = "Resource Not Found",
                Status = StatusCodes.Status404NotFound,
                Instance = context.Request.Path,
                Detail = ex.Message,
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
