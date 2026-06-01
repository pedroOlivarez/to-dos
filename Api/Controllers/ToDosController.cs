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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<PaginatedResponse<ToDoModel>> Get([FromQuery] PaginatedRequest request)
    {
        return await _toDoService.GetMany(request, GetUserId());
    }

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ToDoModel> GetById(int id)
    {
        return await _toDoService.GetById(id, GetUserId());
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ToDoModel> Create(ToDoInsertDto toDoInsertDto)
    {
        if (!toDoInsertDto.IsValid)
        {
            throw new ArgumentException("Could not create to-do. Data not properly structured");
        }
        return await _toDoService.Create(toDoInsertDto, GetUserId());
    }

    [HttpPatch]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ToDoModel> Update(int id, ToDoUpdateDto toDoUpdateDto)
    {
        if (!toDoUpdateDto.IsValid)
        {
            throw new ArgumentException("Could not update to-do. Data not properly structured");
        }
        return await _toDoService.Update(id, toDoUpdateDto, GetUserId());
    }

    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<NoContentResult> Archive(int id)
    {
        await _toDoService.Archive(id, GetUserId());
        return NoContent();
    }
}
