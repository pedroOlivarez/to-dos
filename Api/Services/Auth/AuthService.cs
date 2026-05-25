using System.Security.Claims;
using System.Text;
using Api.Authentication;
using Api.Contracts;
using Api.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Api.Services.Auth;

public class AuthService(
    IConfiguration configuration,
    IPasswordHasher passwordHasher,
    IUserRepository userRepository
) : IAuthService
{
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    private readonly IUserRepository _userRepository = userRepository;

    private readonly string _jwtSecretKey =
        configuration["Jwt:Secret"] ?? throw new ArgumentNullException(nameof(_jwtSecretKey));

    public async Task<string> Authenticate(AuthenticationRequest authenticationRequest)
    {
        try
        {
            var user = await _userRepository.GetByEmail(authenticationRequest.Email);
            var verificationResult = _passwordHasher.VerifyHashedPassword(
                user.Password,
                authenticationRequest.Password
            );
            if (verificationResult == PasswordVerificationResult.Success)
            {
                return GetJwt(user);
            }
            throw new UnauthorizedAccessException();
        }
        // suppress 404 on email not found to not expose that information
        catch (KeyNotFoundException)
        {
            throw new UnauthorizedAccessException();
        }
    }

    private string GetJwt(Entities.User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email.ToString()),
            ]),
            Expires = DateTime.UtcNow.AddMinutes(
                configuration.GetValue<int>("Jwt:ExpirationInMinutes")
            ),
            SigningCredentials = credentials,
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"],
        };

        var handler = new JsonWebTokenHandler();

        return handler.CreateToken(tokenDescriptor);
    }
}
