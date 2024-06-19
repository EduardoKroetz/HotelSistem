using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.EmployeeDTOs;
using Hotel.Domain.Exceptions;
using Hotel.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Handlers.EmployeeHandlers;

public partial class EmployeeHandler
{
    public async Task<Response> HandleUpdateAsync(UpdateEmployee model, Guid id)
    {
        var employee = await _repository.GetEntityByIdAsync(id)
        ?? throw new NotFoundException("Funcionário não encontrado.");

        employee.ChangeName(new Name(model.FirstName, model.LastName));
        employee.ChangePhone(new Phone(model.Phone));
        employee.ChangeGender(model.Gender);
        employee.ChangeDateOfBirth(model.DateOfBirth);
        employee.ChangeAddress(new Address(model.Country, model.City, model.Street, model.Number));
        employee.ChangeSalary(model.Salary ?? employee.Salary);

        try
        {
            _repository.Update(employee);
            await _repository.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            var innerException = e.InnerException?.Message;

            if (innerException != null)
            {

                if (innerException.Contains("Email"))
                    return new Response("Esse email já está cadastrado.");

                if (innerException.Contains("Phone"))
                    return new Response("Esse telefone já está cadastrado.");
            }
        }


        return new Response("Funcionário atualizado com sucesso!", new { employee.Id });
    }
}