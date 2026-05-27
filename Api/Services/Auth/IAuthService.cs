using Api.Models.Auth;

namespace Api.Services.Auth;

public interface IAuthService
{
    Task<bool> Authenticate(AuthenticationRequest authenticationRequest, HttpContext context);
}
