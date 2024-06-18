using Hotel.Domain.DTOs.InvoiceDTOs;
using Hotel.Domain.Entities.InvoiceEntity;

namespace Hotel.Domain.Repositories.Interfaces;

public interface IInvoiceRepository : IRepository<Invoice>, IRepositoryQuery<GetInvoice, InvoiceQueryParameters>
{
}