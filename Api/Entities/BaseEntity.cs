namespace Api.Entities;

public abstract class BaseEntity
{
    public required int Id { get; set; }
    public required bool Archived { get; set; }
}
