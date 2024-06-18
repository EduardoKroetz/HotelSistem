using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.EmployeeDTOs;
using Hotel.Domain.Entities.EmployeeEntity;
using Hotel.Domain.Exceptions;
using Hotel.Domain.Handlers.Base.GenericUserHandler;
using Hotel.Domain.Handlers.Base.Interfaces;
using Hotel.Domain.Repositories.Interfaces;
using Hotel.Domain.Services.EmailServices.Interfaces;
using Hotel.Domain.Services.Permissions;
using Hotel.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Handlers.EmployeeHandlers;

public partial class EmployeeHandler : GenericUserHandler<IEmployeeRepository, Employee>, IHandler
{
    private readonly IEmployeeRepository _repository;
    private readonly IResponsibilityRepository _responsibilityRepository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly IEmailService _emailService;

    public EmployeeHandler(IEmployeeRepository repository, IResponsibilityRepository responsibilityRepository, IPermissionRepository permissionRepository, IEmailService emailService) : base(repository)
    {
        _repository = repository;
        _responsibilityRepository = responsibilityRepository;
        _permissionRepository = permissionRepository;
        _emailService = emailService;
    }

    public async Task<Response> HandleCreateAsync(CreateEmployee model, string? code)
    {
        var email = new Email(model.Email);
        await _emailService.VerifyEmailCodeAsync(email, code);

        DefaultEmployeePermissions.DefaultPermission = await _repository.GetDefaultPermission() ?? throw new NotFoundException("Permissão padrão não encontrada.");

        var employee = new Employee(
          new Name(model.FirstName, model.LastName),
          email,
          new Phone(model.Phone),
          model.Password,
          model.Gender,
          model.DateOfBirth,
          new Address(model.Country, model.City, model.Street, model.Number),
          model.Salary,
          [DefaultEmployeePermissions.DefaultPermission!]
        );

        try
        {
            await _repository.CreateAsync(employee);
            await _repository.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            var innerException = e.InnerException?.Message;

            if (innerException != null)
            {
                if (innerException.Contains("Email"))
                    throw new ArgumentException("Esse email já está cadastrado.");

                if (innerException.Contains("Phone"))
                    throw new ArgumentException("Esse telefone já está cadastrado.");
            }
        }


        return new Response(200, "Funcionário cadastrado com sucesso!", new { employee.Id });
    }
}