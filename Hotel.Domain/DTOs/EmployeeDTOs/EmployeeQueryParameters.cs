using Hotel.Domain.DTOs.Base.User;

namespace Hotel.Domain.DTOs.EmployeeDTOs;

public class EmployeeQueryParameters : UserQueryParameters
{
    public decimal? Salary { get; set; }
    public string? SalaryOperator { get; set; }
}
