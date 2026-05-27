using Api.Contracts;
using Api.Dtos;
using Api.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Api.Middlewares;

public class SystemMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context)
    {
        var traceId = Guid.NewGuid();
        try
        {
            // This will be logging...
            Console.WriteLine(
                $"Method {context.Request.Method} on path {context.Request.Path} called"
            );
            await CheckTokenExiprationAndRefreshIfNecessary(context);
            await _next(context);
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

    // This will require refactoring possibly into sepearte middleware
    private async Task CheckTokenExiprationAndRefreshIfNecessary(HttpContext context)
    {
        Console.WriteLine("Checking token expiration");

        // foreach (var c in context.User.Identities)
        // {
        //     Console.WriteLine($"c.key: {c.Claims} | c.value: {c.Value}");
        // }
        context.Request.Cookies.TryGetValue("refreshToken", out var refreshToken);
        Console.WriteLine($"refreshToken {refreshToken}");
        if (string.IsNullOrWhiteSpace(refreshToken))
            return;

        var expiryString = context.User.Claims.FirstOrDefault(c => c.Type.Equals("exp"))?.Value;
        var userIdString = context
            .User.Claims.FirstOrDefault(c => c.Type.EndsWith("nameidentifier"))
            ?.Value;

        Console.WriteLine($"expiryString: {expiryString}");
        Console.WriteLine($"userIdString: {userIdString}");

        if (string.IsNullOrWhiteSpace(expiryString) || string.IsNullOrWhiteSpace(userIdString))
            return;

        if (
            int.TryParse(expiryString, out int expiry) && int.TryParse(userIdString, out int userId)
        )
        {
            Console.WriteLine("HERE");
            var tokenExpiration = DateTimeOffset.FromUnixTimeSeconds(expiry);
            Console.WriteLine($"tokenExpiration: {tokenExpiration}");
            var diff = tokenExpiration - DateTime.UtcNow;
            Console.WriteLine($"diff: {diff.TotalSeconds}");
            if (diff.TotalSeconds <= 60)
            {
                Console.WriteLine("In here");
                var userRepository =
                    context.RequestServices.GetService<IUserRepository>()
                    ?? throw new Exception("Could not resolve IUserRepository");
                var authService =
                    context.RequestServices.GetService<IAuthService>()
                    ?? throw new Exception("Could not resolve IAuthService");

                var user = await userRepository.GetById(userId);
                if (user is null || user.RefreshToken is null)
                {
                    return;
                }
                if (user.RefreshToken.Equals(refreshToken))
                {
                    context.Response.Cookies.Delete("accessToken");
                    authService.SetAccessToken(authService.GetJwt(user), context);
                    var newRefreshToken = authService.GetRefreshToken();
                    var expiresAt = DateTime.UtcNow.AddDays(1);
                    UserUpdateDto userUpdateDto = new()
                    {
                        RefreshToken = newRefreshToken,
                        RefreshTokenExpiresAt = expiresAt,
                    };
                    await userRepository.Update(user.Id, userUpdateDto);
                    authService.SetRefreshToken(newRefreshToken, expiresAt, context);
                }
            }
        }
    }
}
