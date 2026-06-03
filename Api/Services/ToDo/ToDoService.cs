using Api.Contracts;
using Api.Dtos;
using Api.Models.Shared.Requests;
using Api.Models.Shared.Responses;
using Api.Models.ToDo;
using Api.Utils;

namespace Api.Services.ToDo;

public class ToDoService(IToDoRepository toDoRepository) : IToDoService
{
    private readonly IToDoRepository _toDoRepository = toDoRepository;

    public async Task<ToDoModel> Create(ToDoInsertDto toDoInsertDto, int userId)
    {
        var ToDo = await _toDoRepository.Create(toDoInsertDto, userId);

        return ToDo.ToModel();
    }

    public async Task<ToDoModel> GetById(int toDoId, int userId)
    {
        return (await CheckExistsAndAccess(toDoId, userId)).ToModel();
    }

    public async Task<PaginatedResponse<ToDoModel>> GetMany(PaginatedRequest request, int userId)
    {
        var (total, data) = await _toDoRepository.GetMany(request, userId);
        var maxPages = PageHelper.GetTotalPages(total, request.PageSize);
        if (maxPages < request.Page)
        {
            throw new ArgumentException("Invalid request params");
        }
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

    public async Task<ToDoModel> Update(int toDoId, ToDoUpdateDto toDoUpdateDto, int userId)
    {
        await CheckExistsAndAccess(toDoId, userId);
        await _toDoRepository.Update(toDoId, toDoUpdateDto);

        return (await _toDoRepository.GetById(toDoId)).ToModel();
    }

    public async Task Archive(int id, int userId)
    {
        await CheckExistsAndAccess(id, userId);
        await _toDoRepository.Archive(id);
    }

    private async Task<Entities.ToDo> CheckExistsAndAccess(int id, int userId)
    {
        var toDo = await _toDoRepository.GetById(id);

        if (toDo.UserId != userId)
            throw new UnauthorizedAccessException("User does not have access to this resource");

        return toDo;
    }
}
