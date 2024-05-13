using Hotel.Domain.DTOs.Base;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.EmployeeContext.ResponsabilityDTOs;

public class ResponsabilityQueryParameters : QueryParameters
{
  public ResponsabilityQueryParameters(int? skip, int? take, string? name, DateTime? createdAt, string? createdAtOperator, EPriority? priority, Guid? employeeId, Guid? serviceId) : base(skip,take,createdAt,createdAtOperator)
  {
    Name = name;
    Priority = priority;
    EmployeeId = employeeId;
    ServiceId = serviceId;
  }


  public string? Name { get; private set; }
  public EPriority? Priority{ get; private set; }
  public Guid? EmployeeId { get; private set; }
  public Guid? ServiceId { get; private set; }
}
