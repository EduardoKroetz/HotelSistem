
using Hotel.Domain.Entities.Base.Interfaces;
using Hotel.Domain.Entities.ResponsibilityEntity;
using Hotel.Domain.Enums;

namespace Hotel.Domain.Entities.Interfaces;

public interface IService : IEntity
{
    string Name { get; }
    string Description {  get; }
    decimal Price { get; }
    bool IsActive { get; }
    EPriority Priority { get; }
    int TimeInMinutes { get; }
    string StripeProductId {  get; }
    ICollection<Responsibility> Responsibilities { get; }
}