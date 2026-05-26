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

    [HttpPost]
    [Route("refresh")]
    [Authorize]
    public async Task<StatusCodeResult> Refresh()
    {
        // implement

        // HttpContext.Request.Cookies.TryGetValue("accessToken", out var accessToken);
        // HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken);
        // await _authService.RefreshToken(accessToken, HttpContext);
        return Ok();
    }
}
