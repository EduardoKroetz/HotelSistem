using Hotel.Domain.DTOs;

namespace Hotel.Domain.Handlers.EmployeeContexty.ResponsabilityHandlers;

public partial class ResponsabilityHandler
{
  public async Task<Response<object>> HandleDeleteAsync(Guid id)
  {
    _repository.Delete(id);
    await _repository.SaveChangesAsync();
    return new Response<object>(200,"Responsabilide deletada.", new { id });
  }
}