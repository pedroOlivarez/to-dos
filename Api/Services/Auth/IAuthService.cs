using Api.Models.Auth;

namespace Api.Services.Auth;

public interface IAuthService
{
    Task<bool> Authenticate(AuthenticationRequest authenticationRequest, HttpContext context);
    Task<bool> RefreshToken(string refreshToken, HttpContext context);
    void SetAccessToken(string token, HttpContext context);
    void SetRefreshToken(string token, DateTime expires, HttpContext context);
    string GetJwt(Entities.User user);
    string GetRefreshToken();
}
