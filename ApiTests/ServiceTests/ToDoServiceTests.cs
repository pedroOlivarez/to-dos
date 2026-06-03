using Api.Contracts;
using Api.Dtos;
using Api.Entities;
using Api.Models.Shared.Requests;
using Api.Services.ToDo;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace ApiTests.ServiceTests;

[TestFixture]
public class ToDoServiceTests
{
    private IToDoRepository _toDoRepository;

    private ToDoService _toDoService;

    private static readonly int _user1Id = 10;

    private static readonly int _user2Id = 20;

    private static readonly int _nonexistentToDoId = 1;

    private static readonly int _toDoBelongingToUser1Id = 10;

    private static readonly int _toDoBelongingToUser2Id = 100;

    [SetUp]
    public void Setup()
    {
        _toDoRepository = Substitute.For<IToDoRepository>();

        _toDoRepository
            .GetById(Arg.Is<int>(i => i == _nonexistentToDoId))
            .Throws(new KeyNotFoundException($"ToDo with id: {_nonexistentToDoId} not found"));

        _toDoRepository
            .GetById(Arg.Is<int>(i => i == _toDoBelongingToUser1Id))
            .Returns(
                Task.FromResult(
                    new ToDo
                    {
                        Id = _toDoBelongingToUser1Id,
                        Title = "Doesn't matter",
                        Body = null,
                        UserId = _user1Id,
                        Completed = false,
                        CreatedAt = DateTime.UtcNow.AddDays(-10),
                        UpdatedAt = DateTime.UtcNow.AddDays(-1),
                        Archived = false,
                    }
                )
            );
        _toDoRepository
            .GetById(Arg.Is<int>(i => i == _toDoBelongingToUser2Id))
            .Returns(
                Task.FromResult(
                    new ToDo
                    {
                        Id = _toDoBelongingToUser2Id,
                        Title = "Still doesn't matter",
                        Body = "Why not",
                        UserId = _user2Id,
                        Completed = false,
                        CreatedAt = DateTime.UtcNow.AddDays(-10),
                        UpdatedAt = DateTime.UtcNow.AddDays(-1),
                        Archived = false,
                    }
                )
            );

        _toDoRepository
            .GetMany(Arg.Any<PaginatedRequest>(), Arg.Is<int>(i => i == _user1Id))
            .Returns(
                Task.FromResult<(int, IEnumerable<ToDo>)>(
                    (
                        3,
                        [
                            new ToDo()
                            {
                                Id = 1,
                                Title = "1",
                                UserId = _user1Id,
                                Completed = false,
                                Archived = false,
                            },
                            new ToDo()
                            {
                                Id = 2,
                                Title = "2",
                                UserId = _user1Id,
                                Completed = false,
                                Archived = false,
                            },
                            new ToDo()
                            {
                                Id = 3,
                                Title = "3",
                                UserId = _user1Id,
                                Completed = false,
                                Archived = false,
                            },
                        ]
                    )
                )
            );

        _toDoRepository
            .Update(Arg.Any<int>(), Arg.Any<ToDoUpdateDto>())
            .Returns(Task.CompletedTask);

        _toDoRepository.Archive(Arg.Any<int>()).Returns(Task.CompletedTask);

        _toDoService = new ToDoService(_toDoRepository);
    }

    [Test]
    public async Task GetById_Throws_KeyNotFound_On_NotExists()
    {
        Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            await _toDoService.GetById(_nonexistentToDoId, _user1Id)
        );
    }

    [Test]
    public async Task GetById_Throws_Unauthorized_When_CreatedBy_DoesNotMatch()
    {
        Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
            await _toDoService.GetById(_toDoBelongingToUser1Id, _user2Id)
        );
    }

    [Test]
    public async Task Get_YourOwnToDo_IsFine_AndDandy()
    {
        var result = await _toDoService.GetById(_toDoBelongingToUser1Id, _user1Id);

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public async Task GetToDos_With_BadArgs_ThrowsArgumentException()
    {
        var request = new PaginatedRequest { Page = 2, PageSize = 4 };

        Assert.ThrowsAsync<ArgumentException>(async () =>
            await _toDoService.GetMany(request, _user1Id)
        );
    }

    [Test]
    public async Task Update_DifferentUserToDo_Throws_Unauthorized()
    {
        var updateDto = new ToDoUpdateDto { Body = "updated body" };
        Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
            await _toDoService.Update(_toDoBelongingToUser2Id, updateDto, _user1Id)
        );
    }

    [Test]
    public async Task Update_YourOwnToDo_Is_PerfectlyFine()
    {
        var updateDto = new ToDoUpdateDto { Title = "updated title" };
        var result = await _toDoService.Update(_toDoBelongingToUser1Id, updateDto, _user1Id);

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public async Task Archive_Throws_KeyNotFound_On_NotExists()
    {
        Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            await _toDoService.Archive(_nonexistentToDoId, _user1Id)
        );
    }

    [Test]
    public async Task Archive_Throws_Unauthorized_When_CreatedBy_DoesNotMatch()
    {
        Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
            await _toDoService.Archive(_toDoBelongingToUser1Id, _user2Id)
        );
    }
}
