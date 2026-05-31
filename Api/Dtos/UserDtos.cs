using System.Text.RegularExpressions;

namespace Api.Dtos;

internal static class ValidationHelpers
{
    public static readonly string EmailRegex =
        @"/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/";
    public static readonly string PasswordRegex = @"/\d/";
}

public class UserInsertDto
{
    public required string Email { get; set; }

    public required string Password { get; set; }
    public bool IsValid =>
        !string.IsNullOrWhiteSpace(Email)
        && Regex.IsMatch(Email, ValidationHelpers.EmailRegex)
        && !string.IsNullOrWhiteSpace(Password)
        && Regex.IsMatch(Password, ValidationHelpers.PasswordRegex)
        && Email.Length <= 500
        && Password.Length <= 500;
}

public class UserUpdateDto
{
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiresAt { get; set; }
}
