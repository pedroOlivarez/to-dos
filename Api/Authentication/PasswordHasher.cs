using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;

namespace Api.Authentication;

public class PasswordHasher : IPasswordHasher
{
    private readonly int SaltSize = 16;
    private readonly int KeySize = 32;
    private readonly int Iterations = 10000;
    private static readonly HashAlgorithmName hashAlgorithmName = HashAlgorithmName.SHA256;
    private static readonly char Delimiter = ';';

    public string HashPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password cannot be empty or null");
        }
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            hashAlgorithmName,
            KeySize
        );

        var result = string.Join(
            Delimiter,
            Convert.ToBase64String(salt),
            Convert.ToBase64String(hash)
        );

        return result;
    }

    public PasswordVerificationResult VerifyHashedPassword(
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
