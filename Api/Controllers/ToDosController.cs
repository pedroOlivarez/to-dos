using Api.Dtos;
using Api.Models;
using Api.Services.ToDo;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ToDosController(
    IToDoService toDoService
) : ControllerBase
{
    private readonly IToDoService _toDoService = toDoService;

    [HttpGet(Name = "GetTodos")]
    public async Task<IEnumerable<UserModel>> Get()
    {
        return [];
    }

    [HttpPost(Name = "CreateToDo")]
    public async Task<ToDoModel> Create(ToDoInsertDto toDoInsertDto)
    {
        return await _toDoService.Create(toDoInsertDto);
    }

    // [HttpPatch(Name = "PatchUser")]
    // public async Task<UserModel> Patch(UserUpdateDto userUpdateDto)
    // {
    //     return await _userService.Update(userUpdateDto);
    // }
}
