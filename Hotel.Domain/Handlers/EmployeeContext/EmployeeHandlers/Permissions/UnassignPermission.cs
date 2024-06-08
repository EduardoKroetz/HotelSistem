using Hotel.Domain.DTOs;
using Hotel.Domain.Services.Permissions;

namespace Hotel.Domain.Handlers.EmployeeContext.EmployeeHandlers;

partial class EmployeeHandler
{
  public async Task<Response> HandleUnassignPermission(Guid employeeId, Guid permissionId)
  {
    var employee = await _repository.GetEmployeeIncludesPermissions(employeeId);
    if (employee == null)
      throw new ArgumentException("Funcionário não encontrado.");

    var permission = await _permissionRepository.GetEntityByIdAsync(permissionId);
    if (permission == null)
      throw new ArgumentException("Permissão não encontrada.");

    //Faz verificação se a permissão a ser removida é uma permissão padrão. Se for, vai remover 'DefaultEmployeePermissions'
    //e adicionar todas as permissões padrões menos a removida
    await DefaultEmployeePermissions.HandleDefaultPermission(permission, employee, _repository);

    //Desatribuir a permissão
    employee.UnassignPermission(permission);

    await _repository.SaveChangesAsync();

    return new Response(200, "Permissão removida! Faça login novamente para aplicar as alterações.", null!);
  }
}
