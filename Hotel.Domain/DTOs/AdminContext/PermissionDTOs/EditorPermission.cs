
using Hotel.Domain.DTOs.Interfaces;

namespace Hotel.Domain.DTOs.AdminContext.PermissionDTOs;

public class EditorPermission : IDataTransferObject
{
  public EditorPermission(string name, string description)
  {
    Name = name;
    Description = description;
  }

  public string Name { get; set; }
  public string Description { get; set; }
}