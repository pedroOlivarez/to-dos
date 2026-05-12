using Api.Dtos;
using Api.Models;
using Api.Models.Requests;
using Api.Models.Responses;
using Api.Services.ToDo;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ToDosController(IToDoService toDoService) : ControllerBase
{
    private readonly IToDoService _toDoService = toDoService;

    [HttpGet]
    public async Task<PaginatedResponse<ToDoModel>> Get([FromQuery] PaginatedRequest request)
    {
        return await _toDoService.GetMany(request);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ToDoModel> GetById(int id)
    {
        return await _toDoService.GetById(id);
    }

    [HttpPost]
    public async Task<ToDoModel> Create(ToDoInsertDto toDoInsertDto)
    {
        return await _toDoService.Create(toDoInsertDto);
    }

    [HttpPatch]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ToDoModel> Update(int id, ToDoUpdateDto toDoUpdateDto)
    {
        return await _toDoService.Update(id, toDoUpdateDto);
    }

    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<NoContentResult> Archive(int id)
    {
        await _toDoService.Archive(id);
        return NoContent();
    }
}
