using Api.Dtos;
using Api.Entities;
using Api.Models.Requests;

namespace Api.Contracts;

public interface IToDoRepository
{
   public Task<(int Total, IEnumerable<ToDo>)> GetMany(PaginatedRequest request);

   public Task<ToDo> GetById(int id);

   public Task<ToDo> Create(ToDoInsertDto toDoInsertDto);

   public Task Update(int id, ToDoUpdateDto toDoUpdateDto);

   public Task Archive(int id);
}