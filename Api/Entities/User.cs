using Api.Models.User;

namespace Api.Entities;

public class User : BaseEntity
{
    public required string Email { get; set; }

    public required string Password { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiresAt { get; set; }

    public UserModel ToModel()
    {
        return new UserModel { Id = Id, Email = Email };
    }
}
