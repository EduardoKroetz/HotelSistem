using Hotel.Domain.DTOs.EmployeeContext.ResponsabilityDTOs;
using Hotel.Domain.DTOs.Interfaces;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.RoomContext.ServiceDTOs;

public class GetService : IDataTransferObject
{
  public GetService(Guid id ,string name, decimal price, EPriority priority, bool isActive, int timeInMinutes, ICollection<GetReponsability> responsabilities)
  {
    Id = id;
    Name = name;
    Price = price;
    Priority = priority;
    TimeInMinutes = timeInMinutes;
    IsActive = isActive;
    Responsabilities = responsabilities;
  }

  public Guid Id { get; set; }
  public string Name { get; set; }
  public decimal Price { get; set; }
  public EPriority Priority { get; set; }
  public bool IsActive { get; set; }
  public int TimeInMinutes { get; set; }
  public ICollection<GetReponsability> Responsabilities { get; set; } = [];
} 
  
