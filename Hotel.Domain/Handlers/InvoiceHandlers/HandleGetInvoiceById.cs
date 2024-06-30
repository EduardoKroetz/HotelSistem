using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.InvoiceDTOs;

namespace Hotel.Domain.Handlers.InvoiceHandlers;

public partial class InvoiceHandler
{
    public async Task<Response<GetInvoice>> HandleGetByIdAsync(Guid id)
    {
        var invoice = await _repository.GetByIdAsync(id);
        if (invoice == null)
            throw new ArgumentException("Fatura de quarto não encontrada.");

        return new Response<GetInvoice>("Sucesso!", invoice);
    }
}