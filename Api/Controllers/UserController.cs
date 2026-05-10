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
}
