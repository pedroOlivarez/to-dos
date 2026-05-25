using Api.Dtos;
using Api.Models.User;
using Api.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpGet]
    [Route("{id}")]
    public async Task<UserModel> GetById(int id)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public async Task<UserModel> Create(UserInsertDto userInsertDto)
    {
        return await _userService.Create(userInsertDto);
    }

    [HttpPatch]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<UserModel> Update(int id, UserUpdateDto userUpdateDto)
    {
        return await _userService.Update(id, userUpdateDto);
    }

    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<NoContentResult> Archive(int id)
    {
        await _userService.Archive(id);
        return NoContent();
    }
}
