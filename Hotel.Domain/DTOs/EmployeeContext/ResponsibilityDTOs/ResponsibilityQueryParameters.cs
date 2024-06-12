using Hotel.Domain.DTOs.Base;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.EmployeeContext.ResponsibilityDTOs;

public class ResponsibilityQueryParameters : QueryParameters
{
  public ResponsibilityQueryParameters(int? skip, int? take, string? name,  EPriority? priority, Guid? employeeId, Guid? serviceId, DateTime? createdAt, string? createdAtOperator) : base(skip,take,createdAt,createdAtOperator)
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
