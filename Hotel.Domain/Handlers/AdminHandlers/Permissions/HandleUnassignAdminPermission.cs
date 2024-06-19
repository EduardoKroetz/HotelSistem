
using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;
using Hotel.Domain.Services.Permissions;

namespace Hotel.Domain.Handlers.AdminHandlers;
partial class AdminHandler
{
    public async Task<Response> HandleRemovePermission(Guid adminId, Guid permissionId)
    {
        //Buscar admin
        var admin = await _repository.GetAdminIncludePermissions(adminId)
          ?? throw new NotFoundException("Administrador não encontrado.");

        //Buscar permissão
        var permission = await _permissionRepository.GetEntityByIdAsync(permissionId)
          ?? throw new NotFoundException("Permissão não encontrada.");

        //Faz verificação se a permissão a ser removida é uma permissão padrão. Se for, vai remover 'DefaultAdminPermissions'
        //e adicionar todas as permissões padrões menos a removida
        await DefaultAdminPermissions.HandleDefaultPermission(permission, admin, _repository);

        admin.RemovePermission(permission);

        await _repository.SaveChangesAsync();

        return new Response("Permissão removida! Faça login novamente para aplicar as alterações.");
    }
}
