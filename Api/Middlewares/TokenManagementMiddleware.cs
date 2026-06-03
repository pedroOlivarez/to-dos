using Api.Contracts;
using Api.Dtos;
using Api.Services.Auth;

namespace Api.Middlewares;

public class TokenManagementMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(
        HttpContext context,
        IAuthService authService,
        IUserRepository userRepository
    )
    {
        try
        {
            await CheckTokenExiprationAndRefreshIfNecessary(context, authService, userRepository);
            await _next(context);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _next(context);
        }
    }

    private static async Task CheckTokenExiprationAndRefreshIfNecessary(
        HttpContext context,
        IAuthService authService,
        IUserRepository userRepository
    )
    {
        context.Request.Cookies.TryGetValue("refreshToken", out var refreshToken);
        if (string.IsNullOrWhiteSpace(refreshToken))
            return;
        context.Request.Cookies.TryGetValue("accessToken", out var accessToken);

        if (IsAccessTokenPresentAndValid(accessToken, context))
        {
            return;
        }

        var user = await userRepository.GetByRefreshToken(refreshToken);
        if (
            user == null
            || user.RefreshToken == null
            || user.RefreshTokenExpiresAt == null
            || DateTime.UtcNow > user.RefreshTokenExpiresAt
        )
        {
            return;
        }

        await authService.SetTokens(user, context);
    }

    private static bool IsAccessTokenPresentAndValid(string? accessToken, HttpContext context)
    {
        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            // this whole block might be uneccesary. I'm not sure if a token that is sent up that expires during a request is invalid?
            var expiryString = context.User.Claims.FirstOrDefault(c => c.Type.Equals("exp"))?.Value;
            if (int.TryParse(expiryString, out int expiry))
            {
                var tokenExpiration = DateTimeOffset.FromUnixTimeSeconds(expiry);
                var diff = tokenExpiration - DateTime.UtcNow;
                if (diff.TotalSeconds >= 0)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
