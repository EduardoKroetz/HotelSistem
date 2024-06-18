using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.EmployeeDTOs;

public class CreateEmployee : CreateUser
{
    public CreateEmployee(string firstName, string lastName, string email, string phone, string password, EGender? gender, DateTime? dateOfBirth, string? country, string? city, string? street, int? number, decimal? salary) : base(firstName, lastName, email, phone, password, gender, dateOfBirth, country, city, street, number)
    => Salary = salary;

    public decimal? Salary { get; private set; }
}
