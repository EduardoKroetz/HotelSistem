using Hotel.Domain.Entities.DislikeEntity;
using Hotel.Domain.Entities.InvoiceEntity;
using Hotel.Domain.Entities.LikeEntity;
using Hotel.Domain.Entities.ReservationEntity;
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
