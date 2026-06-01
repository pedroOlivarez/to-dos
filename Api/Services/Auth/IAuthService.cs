using Api.Models.Auth;

namespace Api.Services.Auth;

public interface IAuthService
{
    Task<bool> Authenticate(AuthenticationRequest authenticationRequest, HttpContext context);
    Task LogOut(int userId, HttpContext context);

    Task SetTokens(Entities.User user, HttpContext context, bool expireTokens = false);
}
