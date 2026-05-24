using System.Security.Cryptography;
using Api.Dtos;
using Microsoft.AspNetCore.Identity;

namespace Api.Authentication;

public class PasswordHasher : IPasswordHasher<BaseUserDto>
{
    private readonly int SaltSize = 16;
    private readonly int KeySize = 32;
    private readonly int Iterations = 10000;
    private static readonly HashAlgorithmName hashAlgorithmName = HashAlgorithmName.SHA256;
    private static readonly char Delimiter = ';';

    public string HashPassword(BaseUserDto user, string password)
    {
        var salt = RandomNumberGenerator.GetBytes(count: SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            hashAlgorithmName,
            KeySize
        );

        return string.Join(Delimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
    }

    public PasswordVerificationResult VerifyHashedPassword(
        BaseUserDto user,
        string hashedPassword,
        string providedPassword
    )
    {
        var elements = hashedPassword.Split(Delimiter);
        var salt = Convert.FromBase64String(elements[0]);
        var hash = Convert.FromBase64String(elements[1]);

        var hashedInput = Rfc2898DeriveBytes.Pbkdf2(
            providedPassword,
            salt,
            Iterations,
            hashAlgorithmName,
            KeySize
        );

        return CryptographicOperations.FixedTimeEquals(hash, hashedInput)
            ? PasswordVerificationResult.Success
            : PasswordVerificationResult.Failed;
    }
}
