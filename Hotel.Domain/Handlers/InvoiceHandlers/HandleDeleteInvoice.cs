using Hotel.Domain.DTOs;

namespace Hotel.Domain.Handlers.InvoiceHandlers;

public partial class InvoiceHandler
{
    public async Task<Response> HandleDeleteAsync(Guid id)
    {
        _repository.Delete(id);
        await _repository.SaveChangesAsync();
        return new Response(200, "Fatura deletada com sucesso!", new { id });
    }
}