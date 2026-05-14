using Api.Models;

namespace Api.Entities;

public class ToDo : BaseEntity
{
    public required string Title { get; set; }

    public string? Body { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public required bool Completed { get; set; }

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
