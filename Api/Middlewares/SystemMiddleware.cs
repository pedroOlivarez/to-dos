using Microsoft.AspNetCore.Mvc;

namespace Api.Middlewares;

public class SystemMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var traceId = Guid.NewGuid();
            // we would log stuff here; for now just console log it
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
