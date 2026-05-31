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
        var user = await _userRepository.GetByEmail(authenticationRequest.Email);
        if (user == null)
        {
            return false;
        }
        var verificationResult = _passwordHasher.VerifyHashedPassword(
            user.Password,
            authenticationRequest.Password
        );
        if (verificationResult == PasswordVerificationResult.Success)
        {
            // To-Do (medium DRY)
            // extract to public doodad
            // this is also used in user-registration flow now
            SetAccessToken(GetJwt(user), context);
            var refreshToken = await GetRefreshToken();
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

    public async Task LogOut(int userId, HttpContext context)
    {
        var user =
            await _userRepository.GetById(userId)
            ?? throw new KeyNotFoundException("User not found");

        // To-Do (medium DRY)
        SetAccessToken(GetJwt(user), context, true);
        var refreshToken = await GetRefreshToken();
        var expiresAt = DateTime.UtcNow.AddDays(-1);
        UserUpdateDto userUpdateDto = new()
        {
            RefreshToken = "",
            RefreshTokenExpiresAt = expiresAt,
        };
        await _userRepository.Update(user.Id, userUpdateDto);
        SetRefreshToken(refreshToken, expiresAt, context);
    }

    public void SetAccessToken(string token, HttpContext context, bool expireToken = false)
    {
        context.Response.Cookies.Append(
            "accessToken",
            token,
            new CookieOptions
            {
                Expires = expireToken
                    ? DateTimeOffset.UtcNow.AddDays(-1)
                    : DateTimeOffset.UtcNow.AddMinutes(_jwtExpiry),
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

    public async Task<string> GetRefreshToken()
    {
        var tokenExists = true;
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        var potentialRefreshToken = Convert.ToBase64String(randomNumber);
        while (tokenExists)
        {
            tokenExists = await _userRepository.RefreshTokenExists(potentialRefreshToken);
            if (tokenExists)
            {
                rng.GetBytes(randomNumber);
                potentialRefreshToken = Convert.ToBase64String(randomNumber);
            }
        }
        return potentialRefreshToken;
    }
}
