using Hotel.Domain.DTOs.Base;

namespace Hotel.Domain.DTOs.PermissionDTOs;

public class PermissionQueryParameters : QueryParameters
{
    public string? Name { get; set; }
    public bool? IsActive { get; set; }
    public Guid? AdminId { get; set; }
}
