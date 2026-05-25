using Api.Entities;
using Api.Services.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpGet]
    [Route("google")]
    public async Task<IResult> GoogleLogin(
        [FromQuery] string returnUrl,
        LinkGenerator linkGenerator,
        SignInManager<User> signInManager,
        HttpContext context
    )
    {
        var properties = signInManager.ConfigureExternalAuthenticationProperties(
            "Google",
            linkGenerator.GetPathByName(context, "GoogleLoginCallback") + $"?returnUrl={returnUrl}"
        );

        return Results.Challenge(properties, ["Google"]);
    }

    [HttpGet]
    [Route("google/callback")]
    public async Task<IResult> GoogleCallback(
        [FromQuery] string returnUrl,
        SignInManager<User> signInManager,
        HttpContext context
    )
    {
        var result = await context.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

        if (!result.Succeeded)
        {
            return Results.Unauthorized();
        }

        return Results.Accepted();
    }
}
