using System.Security.Claims;
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

    private int GetUserId()
    {
        var userIdStr = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdStr, out int userId))
        {
            throw new UnauthorizedAccessException();
        }
        return userId;
    }

    [HttpPost]
    [Route("authenticate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<bool> Authenticate(AuthenticationRequest authenticationRequest)
    {
        return await _authService.Authenticate(authenticationRequest, HttpContext);
    }

    [HttpPost]
    [Route("log-out")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [Authorize]
    public async Task<StatusCodeResult> LogOut()
    {
        await _authService.LogOut(GetUserId(), HttpContext);
        return NoContent();
    }

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
