using Api.Models.Auth;
using Api.Models.User;
using Microsoft.AspNetCore.Identity;

namespace Api.Services.Auth;

public interface IAuthService
{
    Task<string> Authenticate(AuthenticationRequest authenticationRequest);
}
