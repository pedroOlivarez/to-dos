namespace Api.Dtos;

public class ToDoInsertDto
{
    public required string Title { get; set; }

    public string? Body { get; set; }

    public bool IsValid =>
        !string.IsNullOrWhiteSpace(Title) && Title.Length <= 500 && Body?.Length <= 1000;
}

public class ToDoUpdateDto
{
    public string? Title { get; set; }

    public string? Body { get; set; }

    public bool? Completed { get; set; }
    public bool IsValid =>
        !string.IsNullOrWhiteSpace(Title) && Title?.Length <= 500 && Body?.Length <= 1000;
}
