using Api.Dtos;
using Api.Models;
using Api.Models.Requests;
using Api.Models.Responses;

namespace Api.Services.ToDo;

public interface IToDoService
{
    Task<PaginatedResponse<ToDoModel>> GetMany(PaginatedRequest request);

    Task<ToDoModel> Create(ToDoInsertDto toDoInsertDto);

    Task<ToDoModel> Update(ToDoUpdateDto toDoUpdateDto);
}