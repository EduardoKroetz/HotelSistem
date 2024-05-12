using Hotel.Domain.DTOs.Interfaces;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.EmployeeContext.ResponsabilityDTOs;

public class EditorResponsability : IDataTransferObject
{
  public EditorResponsability(string name, string description, EPriority priority)
  {
    Name = name;
    Description = description;
    Priority = priority;
  }

  public string Name { get; private set; }
  public string Description { get; private set; } 
  public EPriority Priority { get; private set; }
}

