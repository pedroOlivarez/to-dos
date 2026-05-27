namespace Api.Dtos;

public class UserInsertDto
{
    public required string Email { get; set; }

    public required string Password { get; set; }
}

public class UserUpdateDto
{
    public string? Password { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiresAt { get; set; }
}
