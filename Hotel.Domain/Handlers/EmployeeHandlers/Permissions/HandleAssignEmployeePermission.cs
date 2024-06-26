﻿using Hotel.Domain.DTOs;

namespace Hotel.Domain.Handlers.EmployeeHandlers;
partial class EmployeeHandler
{
    public async Task<Response> HandleAssignPermission(Guid employeeId, Guid permissionId)
    {
        var employee = await _repository.GetEmployeeIncludesPermissions(employeeId);
        if (employee == null)
            throw new ArgumentException("Funcionário não encontrado.");

        var permission = await _permissionRepository.GetEntityByIdAsync(permissionId);
        if (permission == null)
            throw new ArgumentException("Permissão não encontrada.");

        //Atribuir a permissão
        employee.AssignPermission(permission);

        await _repository.SaveChangesAsync();

        return new Response("Permissão adicionada! Faça login novamente para aplicar as alterações.", null!);
    }
}
