using Api.Dtos;
using Api.Entities;
using Api.Models.Shared.Requests;

namespace Api.Contracts;

public interface IToDoRepository
{
    public Task<(int Total, IEnumerable<ToDo>)> GetMany(PaginatedRequest request, int userId);

    public Task<ToDo> GetById(int toDoId);

    public Task<ToDo> Create(ToDoInsertDto toDoInsertDto, int userId);

    public Task Update(int toDoId, ToDoUpdateDto toDoUpdateDto);

    public Task Archive(int id);
}
