using Hotel.Domain.DTOs.Base;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.RoomContext.ReportDTOs;

public class ReportQueryParameters : QueryParameters
{
  public ReportQueryParameters(int? skip, int? take, DateTime? createdAt, string? createdAtOperator, string? summary, EStatus? status, EPriority? priority, Guid? employeeId) : base(skip, take, createdAt, createdAtOperator)
  {
    Summary = summary;
    Status = status;
    Priority = priority;
    EmployeeId = employeeId;
  }

  public string? Summary { get; private set; }
  public EStatus? Status { get; private set; }
  public EPriority? Priority { get; private set; }
  public Guid? EmployeeId { get; private set; }
}