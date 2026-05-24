namespace Api.Entities;

public abstract class BaseEntity
{
    public required int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public required bool Archived { get; set; }
}
