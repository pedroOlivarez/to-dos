using Api.Dtos;
using Api.Models;

namespace Api.Services.ToDo;

public interface IToDoService
{
    Task<IEnumerable<ToDoModel>> GetMany();

    Task<ToDoModel> Create(ToDoInsertDto toDoInsertDto);

    Task<ToDoModel> Update(ToDoUpdateDto toDoUpdateDto);
}