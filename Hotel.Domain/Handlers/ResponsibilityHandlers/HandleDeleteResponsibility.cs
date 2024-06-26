using Hotel.Domain.DTOs;

namespace Hotel.Domain.Handlers.y.ResponsibilityHandlers;

public partial class ResponsibilityHandler
{
    public async Task<Response> HandleDeleteAsync(Guid id)
    {
        _repository.Delete(id);
        await _repository.SaveChangesAsync();
        return new Response("Responsabilide deletada com sucesso!", new { id });
    }
}