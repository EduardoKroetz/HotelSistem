using Hotel.Domain.DTOs;
using Hotel.Domain.Handlers.Interfaces;

namespace Hotel.Domain.Handlers.AdminContext.AdminHandlers;

public partial class AdminHandler : IHandler
{
  public async Task<Response<object>> HandleDeleteAsync(Guid adminId)
  {
    _repository.Delete(adminId);
    await _repository.SaveChangesAsync();
    return new Response<object>(200,"Admin deletado com sucesso!", new { adminId });
  }
}