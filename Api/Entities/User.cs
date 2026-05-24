using Api.Models;

namespace Api.Entities;

public class User : BaseEntity
{
    public required string Email { get; set; }

    public required string Password { get; set; }

    public UserModel ToModel()
    {
        return new UserModel { Id = Id, Email = Email };
    }
}
