
using Hotel.Domain.DTOs.Interfaces;

namespace Hotel.Domain.DTOs.AdminContext.PermissionDTOs;

public class GetPermission : IDataTransferObject
{
  public GetPermission(Guid id, string name, string description, bool isActive)
  {
    Id = id;
    Name = name;
    Description = description;
    IsActive = isActive;
  }

  public Guid Id { get; private set; }
  public string Name { get; private set; }
  public string Description { get; private set; }
  public bool IsActive { get; private set; }
}