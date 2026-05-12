namespace Api.Models;

public class ToDoModel
{
    public required int Id { get; set; }

    public required string Title { get; set; }

    public string? Body { get; set; }

    public required DateTime UpdatedAt { get; set; }
}
