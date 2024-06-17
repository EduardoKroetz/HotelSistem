using Hotel.Domain.DTOs.Interfaces;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.EmployeeContext.ResponsibilityDTOs;

public class GetResponsibility : IDataTransferObject
{
  public GetResponsibility(Guid id,string name, string description, EPriority priority, DateTime createdAt)
  {
    Id = id;
    Name = name;
    Description = description;
    Priority = priority;
    CreatedAt = createdAt;
  }
  public Guid Id { get; private set; }
  public string Name { get; private set; }
  public string Description { get; private set; } 
  public EPriority Priority { get; private set; }
  public DateTime CreatedAt { get; private set; }
}

