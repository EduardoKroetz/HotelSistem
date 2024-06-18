using Hotel.Domain.DTOs.Base;

namespace Hotel.Domain.DTOs.PermissionDTOs;

public class PermissionQueryParameters : QueryParameters
{
    public PermissionQueryParameters(int? skip, int? take, DateTime? createdAt, string? createdAtOperator, string? name, bool? isActive, Guid? adminId) : base(skip, take, createdAt, createdAtOperator)
    {
        Name = name;
        IsActive = isActive;
        AdminId = adminId;
    }

    public string? Name { get; private set; }
    public bool? IsActive { get; private set; }
    public Guid? AdminId { get; private set; }
}
