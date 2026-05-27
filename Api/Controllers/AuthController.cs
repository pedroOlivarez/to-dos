using Api.Models.Auth;
using Api.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost]
    [Route("authenticate")]
    public async Task<bool> Authenticate(AuthenticationRequest authenticationRequest)
    {
        return await _authService.Authenticate(authenticationRequest, HttpContext);
    }

    // Used to check if user is logged in or should be re-directed to login-page
    [HttpGet]
    [Route("check")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [Authorize]
    public async Task<StatusCodeResult> CheckLogin()
    {
        return Ok();
    }
}
