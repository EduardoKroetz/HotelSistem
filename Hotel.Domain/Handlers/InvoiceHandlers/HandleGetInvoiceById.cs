using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.InvoiceDTOs;

namespace Hotel.Domain.Handlers.InvoiceHandlers;

public partial class InvoiceHandler
{
    public async Task<Response<GetInvoice>> HandleGetByIdAsync(Guid id)
    {
        var roomInvoice = await _repository.GetByIdAsync(id);
        if (roomInvoice == null)
            throw new ArgumentException("Fatura de quarto não encontrada.");

        return new Response<GetInvoice>(200, "Sucesso!", roomInvoice);
    }
}