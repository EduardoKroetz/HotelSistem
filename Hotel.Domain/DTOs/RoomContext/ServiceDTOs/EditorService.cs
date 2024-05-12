using Hotel.Domain.DTOs.Interfaces;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.RoomContext.ServiceDTOs;

public class EditorService : IDataTransferObject
{
  public EditorService(string name, decimal price, EPriority priority, int timeInMinutes)
  {
    Name = name;
    Price = price;
    Priority = priority;
    TimeInMinutes = timeInMinutes;
  }

  public string Name { get; private set; }
  public decimal Price { get; private set; }
  public EPriority Priority { get; private set; }
  public int TimeInMinutes { get; private set; }
} 
  
