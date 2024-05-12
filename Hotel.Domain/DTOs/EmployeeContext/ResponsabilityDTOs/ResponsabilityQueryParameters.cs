using Hotel.Domain.DTOs.Base;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.EmployeeContext.ResponsabilityDTOs;

public class ResponsabilityQueryParamaters : QueryParameters
{
  public ResponsabilityQueryParamaters(int? skip, int? take, string? name, DateTime? createdAt, string? createdAtOperator) : base(skip,take,createdAt,createdAtOperator)
  => Name = name;

  public string? Name { get; private set; }
  public EPriority? Priority{ get; private set; }
  public Guid? EmployeeId { get; private set; }
  public Guid? ServiceId { get; private set; }
}
