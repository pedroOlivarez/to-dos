using Api.Models.Auth;
using Api.Models.User;
using Microsoft.AspNetCore.Identity;

namespace Api.Services.Auth;

public interface IAuthService
{
    Task<bool> Authenticate(AuthenticationRequest authenticationRequest, HttpContext context);

    void SetToken(string Token, HttpContext context);
}
