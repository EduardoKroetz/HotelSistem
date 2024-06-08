﻿using Hotel.Domain.DTOs;
using Hotel.Domain.Handlers.Interfaces;

namespace Hotel.Domain.Handlers.EmployeeContext.EmployeeHandlers;

public partial class EmployeeHandler : IHandler
{
  public async Task<Response> HandleAssignResponsabilityAsync(Guid id, Guid responsabilityId)
  {
    var employee = await _repository.GetEmployeeIncludesResponsabilities(id);
    if (employee == null)
      throw new ArgumentException("Funcionário não encontrado.");

    var responsability = await _responsabilityRepository.GetEntityByIdAsync(responsabilityId);
    if (responsability == null)
      throw new ArgumentException("Responsabilidade não encontrada.");

    employee.AddResponsability(responsability);
    
    await _repository.SaveChangesAsync();

    return new Response(200, "Responsabilidade atribuida com sucesso!");
  }
}