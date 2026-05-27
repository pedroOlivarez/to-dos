using Api.Dtos;
using Api.Models.Shared.Requests;
using Api.Models.Shared.Responses;
using Api.Models.ToDo;

namespace Api.Services.ToDo;

public interface IToDoService
{
    Task<PaginatedResponse<ToDoModel>> GetMany(PaginatedRequest request);

    Task<ToDoModel> GetById(int id);

    Task<ToDoModel> Create(ToDoInsertDto toDoInsertDto);

    Task<ToDoModel> Update(int id, ToDoUpdateDto toDoUpdateDto);

    Task Archive(int id);
}
