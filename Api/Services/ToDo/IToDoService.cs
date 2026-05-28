using Api.Dtos;
using Api.Models.Shared.Requests;
using Api.Models.Shared.Responses;
using Api.Models.ToDo;

namespace Api.Services.ToDo;

public interface IToDoService
{
    Task<PaginatedResponse<ToDoModel>> GetMany(PaginatedRequest request, int userId);

    Task<ToDoModel> GetById(int toDoId, int userId);

    Task<ToDoModel> Create(ToDoInsertDto toDoInsertDto, int userId);

    Task<ToDoModel> Update(int toDoId, ToDoUpdateDto toDoUpdateDto, int userId);

    Task Archive(int toDoId, int userId);
}
