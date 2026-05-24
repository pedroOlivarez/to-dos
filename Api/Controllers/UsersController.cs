using Api.Dtos;
using Api.Models;
using Api.Models.Requests;
using Api.Models.Responses;
using Api.Services.ToDo;
using Api.Services.User;
using Microsoft.AspNetCore.Identity;
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

    // extract to auth controller
    [HttpPost]
    [Route("authenticate")]
    public async Task<StatusCodeResult> Authenticate(AuthenticationRequest authenticationRequest)
    {
        var result = await _userService.Authenticate(authenticationRequest);
        if (result == PasswordVerificationResult.Success)
        {
            return Ok();
        }

        return Unauthorized();
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
