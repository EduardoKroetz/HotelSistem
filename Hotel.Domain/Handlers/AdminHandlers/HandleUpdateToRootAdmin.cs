﻿using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.AdminHandlers;

public partial class AdminHandler
{
    public async Task<Response> HandleChangeToRootAdminAsync(Guid rootAdminId, Guid changeToRootAdminId)
    {
        var changeToRootAdmin = await _repository.GetEntityByIdAsync(changeToRootAdminId)
            ?? throw new NotFoundException("Administrador não encontrado.");

        var typeId = typeof(Guid) == rootAdminId.GetType();
        var rootAdmin = await _repository.GetEntityByIdAsync(rootAdminId)
            ?? throw new NotFoundException("Administrador root não encontrado.");

        changeToRootAdmin.ChangeToRootAdmin(rootAdmin);

        await _repository.SaveChangesAsync();

        return new Response($"O Administrador {changeToRootAdmin.Name.FirstName} é agora um Administrador root.");
    }
}