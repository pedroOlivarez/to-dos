using System.Security.Claims;
using Api.Dtos;
using Api.Models.Shared.Requests;
using Api.Models.Shared.Responses;
using Api.Models.ToDo;
using Api.Services.ToDo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ToDosController(IToDoService toDoService) : ControllerBase
{
    private readonly IToDoService _toDoService = toDoService;

    private int GetUserId()
    {
        var userIdStr = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdStr, out int userId))
        {
            throw new UnauthorizedAccessException();
        }
        return userId;
    }

    [HttpGet]
    public async Task<PaginatedResponse<ToDoModel>> Get([FromQuery] PaginatedRequest request)
    {
        return await _toDoService.GetMany(request, GetUserId());
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ToDoModel> GetById(int id)
    {
        return await _toDoService.GetById(id, GetUserId());
    }

    [HttpPost]
    public async Task<ToDoModel> Create(ToDoInsertDto toDoInsertDto)
    {
        return await _toDoService.Create(toDoInsertDto, GetUserId());
    }

    [HttpPatch]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ToDoModel> Update(int id, ToDoUpdateDto toDoUpdateDto)
    {
        return await _toDoService.Update(id, toDoUpdateDto, GetUserId());
    }

    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<NoContentResult> Archive(int id)
    {
        await _toDoService.Archive(id, GetUserId());
        return NoContent();
    }
}
