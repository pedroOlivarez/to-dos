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
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<UserModel> Register(UserInsertDto userInsertDto)
    {
        var exists = await _userService.CheckIfEmailInUse(userInsertDto.Email);
        if (exists)
            throw new ArgumentException("A user with this email is already registered");

        var user = await _userService.Create(userInsertDto, HttpContext);

        return user;
    }

    // To-Do (medium): Delete this and code for this path
    [HttpPatch]
    [Route("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<UserModel> Update(int id, UserUpdateDto userUpdateDto)
    {
        return await _userService.Update(id, userUpdateDto);
    }

    // To-Do (medium): Delete this and code for this path
    [HttpDelete]
    [Route("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<NoContentResult> Archive(int id)
    {
        await _userService.Archive(id);
        return NoContent();
    }
}
