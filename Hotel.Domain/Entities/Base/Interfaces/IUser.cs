using Hotel.Domain.Enums;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Entities.Base.Interfaces;

public interface IUser : IEntity
{
  Name Name { get; }
  Email Email { get; }
  Phone Phone { get; }
  string? PasswordHash { get; }
  EGender? Gender { get; }
  DateTime? DateOfBirth { get; }
  Address? Address { get; }
  bool IncompleteProfile { get; }
}
