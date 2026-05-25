using Api.Models.Auth;
using Api.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost]
    [Route("authenticate")]
    public async Task<AuthenticationResponse> Authenticate(
        AuthenticationRequest authenticationRequest
    )
    {
        return await _authService.Authenticate(authenticationRequest);
    }
}
