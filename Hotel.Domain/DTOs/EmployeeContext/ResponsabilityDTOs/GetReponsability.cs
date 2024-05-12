using Hotel.Domain.DTOs.Interfaces;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.EmployeeContext.ResponsabilityDTOs;

public class GetReponsability : IDataTransferObject
{
  public GetReponsability(Guid id,string name, string description, EPriority priority)
  {
    Id = id;
    Name = name;
    Description = description;
    Priority = priority;
  }
  public Guid Id { get; set; }
  public string Name { get; set; }
  public string Description { get; set; } 
  public EPriority Priority { get; set; }
}

