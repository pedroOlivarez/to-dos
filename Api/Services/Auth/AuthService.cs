using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Api.Authentication;
using Api.Contracts;
using Api.Dtos;
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
    private readonly int _jwtExpiry =
        configuration.GetValue<int?>("Jwt:ExpirationInMinutes")
        ?? throw new ArgumentNullException(nameof(_jwtExpiry));

    public async Task<bool> Authenticate(
        AuthenticationRequest authenticationRequest,
        HttpContext context
    )
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
                // extract to private doodad
                SetAccessToken(GetJwt(user), context);
                var refreshToken = GetRefreshToken();
                var expiresAt = DateTime.UtcNow.AddDays(1);
                UserUpdateDto userUpdateDto = new()
                {
                    RefreshToken = refreshToken,
                    RefreshTokenExpiresAt = expiresAt,
                };
                await _userRepository.Update(user.Id, userUpdateDto);
                SetRefreshToken(refreshToken, expiresAt, context);
                return true;
            }
            return false;
        }
        // suppress 404 on email not found to not expose that information
        catch (KeyNotFoundException)
        {
            return false;
        }
    }

    public void SetAccessToken(string token, HttpContext context)
    {
        context.Response.Cookies.Append(
            "accessToken",
            token,
            new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddMinutes(_jwtExpiry),
                HttpOnly = true,
                IsEssential = true,
                Secure = true,
                SameSite = SameSiteMode.None,
            }
        );
    }

    public void SetRefreshToken(string token, DateTime expires, HttpContext context)
    {
        context.Response.Cookies.Append(
            "refreshToken",
            token,
            new CookieOptions
            {
                Expires = expires,
                HttpOnly = true,
                IsEssential = true,
                Secure = true,
                SameSite = SameSiteMode.None,
            }
        );
    }

    public string GetJwt(Entities.User user)
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

    public string GetRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    }
}
