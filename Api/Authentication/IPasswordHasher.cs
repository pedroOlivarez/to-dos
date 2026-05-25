using Microsoft.AspNetCore.Identity;

namespace Api.Authentication;

public interface IPasswordHasher
{
    string HashPassword(string password);

    PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword);
}
