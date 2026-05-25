namespace Api.Models.Auth;

public class AuthenticationRequest
{
    public required string Email { get; set; }

    public required string Password { get; set; }
}
