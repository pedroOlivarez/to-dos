namespace Api.Entities;

public class ToDo: BaseEntity
{
   public required string Title { get; set; }

   public string? Body { get; set; }

   public DateOnly CreatedAt { get; set; }

   public required int CreatedBy { get; set; }
}