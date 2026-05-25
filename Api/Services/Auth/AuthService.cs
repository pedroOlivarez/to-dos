using Api.Authentication;
using Api.Contracts;
using Api.Models.Auth;
using Microsoft.AspNetCore.Identity;

namespace Api.Services.Auth;

public class AuthService(IPasswordHasher passwordHasher, IUserRepository userRepository)
    : IAuthService
{
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    private readonly IUserRepository _userRepository = userRepository;

    public async Task<PasswordVerificationResult> Authenticate(
        AuthenticationRequest authenticationRequest
    )
    {
        try
        {
            var user = await _userRepository.GetByEmail(authenticationRequest.Email);
            return _passwordHasher.VerifyHashedPassword(
                user.Password,
                authenticationRequest.Password
            );
        }
        catch
        {
            // don't want to return 404 here. Security concern. Just do 401 or 403 whichever one is bad auth
            return PasswordVerificationResult.Failed;
        }
    }
}
