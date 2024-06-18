using Hotel.Domain.DTOs.Base;

namespace Hotel.Domain.DTOs.PermissionDTOs;

public class GetPermission : GetDataTransferObject
{
    public GetPermission(Guid id, string name, string description, bool isActive, DateTime createdAt) : base(id, createdAt)
    {
        Name = name;
        Description = description;
        IsActive = isActive;
    }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; }
}