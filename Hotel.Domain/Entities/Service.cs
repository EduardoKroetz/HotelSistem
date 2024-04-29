using Hotel.Domain.Enums;

namespace Hotel.Domain.Entities;

public class Service : Entity
{
  public string Name { get; private set; }
  public decimal Price { get; private set; }
  public bool IsActive { get; private set; }
  public EPriority Priority { get; private set; }
}