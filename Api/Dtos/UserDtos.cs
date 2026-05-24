namespace Api.Dtos;

public class UserInsertDto : BaseUserDto
{
    public required string Email { get; set; }
}

public class UserUpdateDto : BaseUserDto { }

public abstract class BaseUserDto
{
    public required string Password { get; set; }
}
