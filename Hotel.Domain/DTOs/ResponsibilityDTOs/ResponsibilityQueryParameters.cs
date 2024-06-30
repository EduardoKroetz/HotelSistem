using Hotel.Domain.DTOs.Base;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.ResponsibilityDTOs;

public class ResponsibilityQueryParameters : QueryParameters
{
    public string? Name { get; set; }
    public EPriority? Priority { get; set; }
    public Guid? EmployeeId { get; set; }
    public Guid? ServiceId { get; set; }
}
