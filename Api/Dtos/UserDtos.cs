namespace Api.Dtos;

public class UserInsertDto : BaseUserDto { }

public class UserUpdateDto : BaseUserDto { }

public abstract class BaseUserDto
{
    public required string Email { get; set; }
}
