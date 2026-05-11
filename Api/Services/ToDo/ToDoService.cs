using Api.Contracts;
using Api.Dtos;
using Api.Models;
using Api.Models.Requests;
using Api.Models.Responses;

namespace Api.Services.ToDo;

public class ToDoService(
    IToDoRepository toDoRepository
) : IToDoService
{
    private readonly IToDoRepository _toDoRepository = toDoRepository;
    public async Task<ToDoModel> Create(ToDoInsertDto toDoInsertDto)
    {
        var ToDo = await _toDoRepository.Create(toDoInsertDto);

        return ToDo.ToModel();
    }

    public async Task<ToDoModel> GetById(int id)
    {
        return (await _toDoRepository.GetById(id)).ToModel();
    }

    public async Task<PaginatedResponse<ToDoModel>> GetMany(PaginatedRequest request)
    {
        var (total, data) = await _toDoRepository.GetMany(request);
        return new PaginatedResponse<ToDoModel>
        {
            Data = data.Select(d => d.ToModel()),
            Meta = new Meta
            {
                Page = request.Page,
                PageSize = request.PageSize,
                Count = data.Count(),
                Total = total
            }
        };
    }

    public async Task<ToDoModel> Update(int id, ToDoUpdateDto toDoUpdateDto)
    {
        await _toDoRepository.Update(id, toDoUpdateDto);

        return (await _toDoRepository.GetById(id)).ToModel();
    }

    public async Task Archive(int id)
    {
        await _toDoRepository.Archive(id);
    }
}