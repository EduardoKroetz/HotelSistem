using Hotel.Domain.DTOs;

namespace Hotel.Domain.Handlers.AdminContext.AdminHandlers;

public partial class AdminHandler
{
  public async Task<Response> HandleDeleteAsync(Guid adminId)
  {
    _repository.Delete(adminId);
    await _repository.SaveChangesAsync();
    return new Response(200,"Administrador deletado com sucesso!");
  }
}