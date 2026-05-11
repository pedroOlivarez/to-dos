namespace Api.Entities;

public class ToDo: BaseEntity
{
   public required string Title { get; set; }

   public string? Body { get; set; }

   public DateTime CreatedAt { get; set; }

   public DateTime UpdatedAt { get; set; }
}