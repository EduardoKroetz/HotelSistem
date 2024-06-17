using Hotel.Domain.DTOs.Interfaces;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.EmployeeContext.ResponsibilityDTOs;

public class EditorResponsibility : IDataTransferObject
{
  public EditorResponsibility(string name, string description, EPriority priority)
  {
    Name = name;
    Description = description;
    Priority = priority;
  }

  public string Name { get; private set; }
  public string Description { get; private set; } 
  public EPriority Priority { get; private set; }
}

