using Api.Models.Auth;
using Microsoft.AspNetCore.Identity;

namespace Api.Services.Auth;

public interface IAuthService
{
    Task<PasswordVerificationResult> Authenticate(AuthenticationRequest authenticationRequest);
}
