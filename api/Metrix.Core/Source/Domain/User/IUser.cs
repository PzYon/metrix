namespace Metrix.Core.Domain.User;

public interface IUser
{
  string? Id { get; set; }

  string Name { get; set; }

  string? DisplayName { get; set; }

  string? ImageUrl { get; set; }

  DateTime? LastLoginDate { get; set; }
}
