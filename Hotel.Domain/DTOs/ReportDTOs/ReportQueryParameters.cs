using Hotel.Domain.DTOs.Base;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.ReportDTOs;

public class ReportQueryParameters : QueryParameters
{
    public string? Summary { get; set; }
    public EStatus? Status { get; set; }
    public EPriority? Priority { get; set; }
    public Guid? EmployeeId { get; set; }
}