using Api.Contracts;
using Api.Dtos;
using Api.Models;

namespace Api.Services.ToDo;

public class ToDoService(
    IToDoRepository toDoRepository
) : IToDoService
{
    private readonly IToDoRepository _toDoRepository = toDoRepository;
    public async Task<ToDoModel> Create(ToDoInsertDto toDoInsertDto)
    {
        var ToDo = await _toDoRepository.Create(toDoInsertDto);

        return new ToDoModel
        {
            Id = ToDo.Id,
            Title = ToDo.Title,
            Body = ToDo.Body,
            UpdatedAt = ToDo.UpdatedAt,
        };
    }

    public Task<IEnumerable<ToDoModel>> GetMany()
    {
        throw new NotImplementedException();
    }

    public Task<ToDoModel> Update(ToDoUpdateDto toDoUpdateDto)
    {
        throw new NotImplementedException();
    }
}