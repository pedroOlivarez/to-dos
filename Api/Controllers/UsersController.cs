using Api.Dtos;
using Api.Models.User;
using Api.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<UserModel> Register(UserInsertDto userInsertDto)
    {
        var exists = await _userService.CheckIfEmailInUse(userInsertDto.Email);
        if (exists)
            throw new ArgumentException("A user with this email is already registered");

        var user = await _userService.Create(userInsertDto, HttpContext);

        return user;
    }
}
