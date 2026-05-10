namespace Api.Entities;

public class User : BaseEntity
{
   public required string FirstName { get; set; }
   public required string LastName { get; set; }
   public required string Email { get; set; }
}