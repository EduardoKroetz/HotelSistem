﻿using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.EmployeeDTOs;

public class UpdateEmployee : UpdateUser
{
    public UpdateEmployee(string firstName, string lastName, string phone, EGender? gender, DateTime? dateOfBirth, string? country, string? city, string? street, int? number, decimal? salary) : base(firstName, lastName, phone, gender, dateOfBirth, country, city, street, number)
      => Salary = salary;

    public decimal? Salary { get; private set; }


}
