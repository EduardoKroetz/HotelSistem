using Hotel.Domain.DTOs;


namespace Hotel.Domain.Handlers.AdminContext.AdminHandlers;
partial class AdminHandler
{
  public async Task<Response<object>> HandleGet(Guid adminId)
  {
    //Buscar admin
    var admin = await _repository.GetAdminIncludePermissions(adminId);
    if (admin == null)
      throw new ArgumentException("Admin não encontrado.");

    return new Response<object>(200, "Admin encontrado.", null!);
  }
}
