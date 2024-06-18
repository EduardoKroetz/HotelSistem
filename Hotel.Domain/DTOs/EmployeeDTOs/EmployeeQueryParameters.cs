using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.EmployeeDTOs;

public class EmployeeQueryParameters : UserQueryParameters
{
    public EmployeeQueryParameters(int? skip, int? take, string? name, string? email, string? phone, EGender? gender, DateTime? dateOfBirth, string? dateOfBirthOperator, DateTime? createdAt, string? createdAtOperator, decimal? salary, string? salaryOperator) : base(skip, take, name, email, phone, gender, dateOfBirth, dateOfBirthOperator, createdAt, createdAtOperator)
    {
        Salary = salary;
        SalaryOperator = salaryOperator;
    }


    public decimal? Salary { get; private set; }
    public string? SalaryOperator { get; private set; }
}
