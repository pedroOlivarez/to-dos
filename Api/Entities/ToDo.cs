using Api.Models;

namespace Api.Entities;

public class ToDo : BaseEntity
{
    public required string Title { get; set; }

    public string? Body { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public ToDoModel ToModel()
    {
        return new ToDoModel
        {
            Id = this.Id,
            Title = this.Title,
            Body = this.Body,
            UpdatedAt = this.UpdatedAt,
        };
    }
}
