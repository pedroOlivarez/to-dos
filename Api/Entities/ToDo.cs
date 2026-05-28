using Api.Models.ToDo;

namespace Api.Entities;

public class ToDo : BaseEntity
{
    public required string Title { get; set; }

    public string? Body { get; set; }

    public required bool Completed { get; set; }

    public required int UserId { get; set; }

    public ToDoModel ToModel()
    {
        return new ToDoModel
        {
            Id = Id,
            Title = Title,
            Body = Body,
            UpdatedAt = UpdatedAt,
            Completed = Completed,
        };
    }
}
