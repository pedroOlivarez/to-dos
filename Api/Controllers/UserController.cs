using Api.Dtos;
using Api.Models;
using Api.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(
    IUserService userService
) : ControllerBase
{
    private readonly  IUserService _userService = userService;

    [HttpGet(Name = "GetUsers")]
    public async Task<IEnumerable<UserModel>> Get()
    {
        return await _userService.GetMany();
    }

    [HttpPost(Name = "CreateUser")]
    public async Task<UserModel> Create(UserInsertDto userInsertDto)
    {
        return await _userService.Create(userInsertDto);
    }

    [HttpPatch(Name = "PatchUser")]
    public async Task<UserModel> Patch(UserUpdateDto userUpdateDto)
    {
        return await _userService.Update(userUpdateDto);
    }
}
