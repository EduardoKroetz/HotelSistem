using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.InvoiceDTOs;

namespace Hotel.Domain.Handlers.InvoiceHandlers;

public partial class InvoiceHandler
{
    public async Task<Response<IEnumerable<GetInvoice>>> HandleGetAsync(InvoiceQueryParameters queryParameters)
    {
        var roomInvoices = await _repository.GetAsync(queryParameters);
        return new Response<IEnumerable<GetInvoice>>("Sucesso!", roomInvoices);
    }
}