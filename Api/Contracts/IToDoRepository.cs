using Api.Dtos;
using Api.Entities;

namespace Api.Contracts;

public interface IToDoRepository
{
   public Task<IEnumerable<ToDo>> GetMany();

   public Task<ToDo> GetById(int id);

   public Task<ToDo> Create(ToDoInsertDto toDoInsertDto);

   public Task Update(int id, ToDoUpdateDto toDoUpdateDto);
}