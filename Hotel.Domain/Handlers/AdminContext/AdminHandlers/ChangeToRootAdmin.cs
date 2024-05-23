﻿using Hotel.Domain.DTOs;

namespace Hotel.Domain.Handlers.AdminContext.AdminHandlers;

public partial class AdminHandler
{
  public async Task<Response<object>> HandleChangeToRootAdminAsync(Guid rootAdminId,Guid changeToRootAdminId)
  {
    var rootAdmin = await _repository.GetEntityByIdAsync(rootAdminId);
    if (rootAdmin == null)
      throw new ArgumentException("Administrador raiz não encontrado.");

    var changeToRootAdmin = await _repository.GetEntityByIdAsync(changeToRootAdminId);
    if (changeToRootAdmin == null)
      throw new ArgumentException("Administrador não encontrado.");

    changeToRootAdmin.ChangeToRootAdmin(rootAdmin);

    await _repository.SaveChangesAsync();

    return new Response<object>(200, $"O Administrador {changeToRootAdmin.Name.FirstName} é agora um Administrador raiz.");
  }
}