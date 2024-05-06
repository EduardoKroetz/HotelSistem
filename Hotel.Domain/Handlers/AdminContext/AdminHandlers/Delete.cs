using Hotel.Domain.DTOs;

namespace Hotel.Domain.Handlers.AdminContext.AdminHandlers;

public partial class AdminHandler
{
  public async Task<Response<object>> HandleDeleteAsync(Guid adminId)
  {
    _repository.Delete(adminId);
    await _repository.SaveChangesAsync();
    return new Response<object>(200,"Admin deletado com sucesso!", new { adminId });
  }
}