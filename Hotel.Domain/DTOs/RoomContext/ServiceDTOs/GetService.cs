using Hotel.Domain.DTOs.EmployeeContext.ResponsibilityDTOs;
using Hotel.Domain.DTOs.Interfaces;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.RoomContext.ServiceDTOs;

public class GetService : IDataTransferObject
{
  public GetService(Guid id ,string name, decimal price, EPriority priority, bool isActive, int timeInMinutes, ICollection<GetResponsibility> responsibilities, DateTime createdAt)
  {
    Id = id;
    Name = name;
    Price = price;
    Priority = priority;
    TimeInMinutes = timeInMinutes;
    IsActive = isActive;
    Responsabilities = responsibilities;
    CreatedAt = createdAt;
  }

  public Guid Id { get; private set; }
  public string Name { get; private set; }
  public decimal Price { get; private set; }
  public EPriority Priority { get; private set; }
  public bool IsActive { get; private set; }
  public int TimeInMinutes { get; private set; }
  public ICollection<GetResponsibility> Responsabilities { get; private set; } = [];
  public DateTime CreatedAt { get; private set; }
}

