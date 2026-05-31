using Api.Models.Auth;

namespace Api.Services.Auth;

public interface IAuthService
{
    Task<bool> Authenticate(AuthenticationRequest authenticationRequest, HttpContext context);
    Task LogOut(int userId, HttpContext context);
    void SetAccessToken(string token, HttpContext context, bool expireToken = false);
    void SetRefreshToken(string token, DateTime expires, HttpContext context);
    string GetJwt(Entities.User user);
    Task<string> GetRefreshToken();
}
