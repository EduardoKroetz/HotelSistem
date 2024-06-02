using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.EmployeeContext.EmployeeDTOs;
using Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;
using Hotel.Domain.Handlers.Base.GenericUserHandler;
using Hotel.Domain.Handlers.Interfaces;
using Hotel.Domain.Repositories.Interfaces.AdminContext;
using Hotel.Domain.Repositories.Interfaces.EmployeeContext;
using Hotel.Domain.Services.Permissions;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Handlers.EmployeeContext.EmployeeHandlers;

public partial class EmployeeHandler : GenericUserHandler<IEmployeeRepository,Employee> ,IHandler
{
  private readonly IEmployeeRepository  _repository;
  private readonly IResponsabilityRepository _responsabilityRepository;
  private readonly IPermissionRepository _permissionRepository;

  public EmployeeHandler(IEmployeeRepository repository, IResponsabilityRepository responsabilityRepository, IPermissionRepository permissionRepository) : base(repository)
  {
    _repository = repository;
    _responsabilityRepository = responsabilityRepository;
    _permissionRepository = permissionRepository;
  }

  public async Task<Response> HandleCreateAsync(CreateEmployee model)
  {
    DefaultEmployeePermissions.DefaultPermission = DefaultEmployeePermissions.DefaultPermission ?? await _repository.GetDefaultPermission();

    var employee = new Employee(
      new Name(model.FirstName,model.LastName),
      new Email(model.Email),
      new Phone(model.Phone),
      model.Password,
      model.Gender,
      model.DateOfBirth,
      new Address(model.Country,model.City,model.Street,model.Number),
      model.Salary,
      [DefaultEmployeePermissions.DefaultPermission!]
    );

    await _repository.CreateAsync(employee);
    await _repository.SaveChangesAsync();

    return new Response(200,"Funcionário foi cadastrado.",new { employee.Id });
  }
}