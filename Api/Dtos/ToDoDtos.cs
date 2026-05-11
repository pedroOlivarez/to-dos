namespace Api.Dtos;

public class ToDoInsertDto
{
    public required string Title { get; set; }

    public string? Body { get; set; }
}

public class ToDoUpdateDto
{
    public string? Title { get; set; }

    public string? Body { get; set; }
}