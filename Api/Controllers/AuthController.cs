using Api.Models.Auth;
using Api.Services.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost]
    [Route("authenticate")]
    public async Task<StatusCodeResult> Authenticate(AuthenticationRequest authenticationRequest)
    {
        var result = await _authService.Authenticate(authenticationRequest);
        if (result == PasswordVerificationResult.Success)
        {
            return Ok();
        }

        return Unauthorized();
    }
}
