using Api.Contracts;
using Api.Dtos;
using Api.Models.Shared.Requests;
using Api.Models.Shared.Responses;
using Api.Models.ToDo;

namespace Api.Services.ToDo;

public class ToDoService(IToDoRepository toDoRepository) : IToDoService
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
                Total = total,
            },
        };
    }

    public async Task<ToDoModel> Update(int id, ToDoUpdateDto toDoUpdateDto)
    {
        await CheckExists(id);
        await _toDoRepository.Update(id, toDoUpdateDto);

        return (await _toDoRepository.GetById(id)).ToModel();
    }

    public async Task Archive(int id)
    {
        await CheckExists(id);
        await _toDoRepository.Archive(id);
    }

    private async Task CheckExists(int id)
    {
        var exists = (await _toDoRepository.GetById(id)) is not null;
        if (!exists)
            throw new Exception("Not Found");
    }
}
